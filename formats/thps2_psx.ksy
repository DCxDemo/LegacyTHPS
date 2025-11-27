meta:
  id: thps_psx
  application: Tony Hawk's Pro Skater
  title: Big Guns engine model container
  file-extension: psx
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyTHPS/blob/master/formats/thps2_psx.ksy

doc: |
  This spec is meant to fully parse PSX files found in multiple games built on 
  Neverosft's "Big guns" engine games including:
  - THPS 1,2,3,4 (release and beta versions)
  - Spiderman 1, 2
  - Apocalypse
  - Mat Hoffman's Pro BMX
  
  PSX files are containers for both models and textures. However some versions
  may split those in several files.
  
  Example:
  SKWARE.TRG - "trigger" file, used for level nodes, links to other files.
  SKWARE.PSX - level mesh
  SKWARE_L.PSX - textures
  SKWARE_O.PSX - additional items used in career mode + skybox
  
  This spec combines the research of dcxdemo and iamgreaser:
  http://thmods.com/forum/viewtopic.php?f=5&t=200&p=1079
  https://gist.github.com/iamgreaser/b54531e41d77b69d7d13391deb0ac6a5

seq:
  - id: version
    type: u2
    enum: version_id
  - id: trg_version
    type: u2 # always 2
  - id: ptr_ext
    type: u4

  - id: num_objects
    type: u4
  - id: objects
    type: obj
    repeat: expr
    repeat-expr: num_objects

  - id: num_models
    type: u4
  - id: ptr_models
    type: u4
    repeat: expr
    repeat-expr: num_models
  - id: models
    type: model
    repeat: expr
    repeat-expr: num_models

  - id: extensions
    type: extension
    repeat: until
    repeat-until: _.ext_type == -1

  - id: model_checksums
    type: u4
    repeat: expr
    repeat-expr: num_models

    # important: level files declare only the textures they use
    # but acutal textures are saved in a texture library.
  - id: num_texture_checksums
    type: u4
  - id: texture_checksums
    type: u4
    repeat: expr
    repeat-expr: num_texture_checksums

  - id: pal16
    type: palette_set(16)

  - id: pal256
    type: palette_set(256)

  - id: textures
    type: texture_set

types:
  extension:
    seq:
      - id: ext_type
        type: s4
      - id: size
        type: u4
        if: ext_type != -1
      - id: data
        size: size
        if: ext_type != -1
  obj:
    seq:
      - id: u1
        type: u4
      - id: position
        type: vec4s4
      - id: u6
        type: u2
      - id: model_index
        type: u2
      - id: u71
        type: u2
      - id: u72
        type: u2
      - id: u8
        type: u4
      - id: ptr_clut
        type: u4

  model:
    seq:
      - id: flags # i guess, mostly 8 and 10.
        type:
          switch-on: _root.version
          cases:
            'version_id::apoc': u4
            'version_id::thps': u2
      - id: num_vertices
        type:
          switch-on: _root.version
          cases:
            'version_id::apoc': u4
            'version_id::thps': u2
      - id: num_normals
        type:
          switch-on: _root.version
          cases:
            'version_id::apoc': u4
            'version_id::thps': u2
      - id: num_faces
        type:
          switch-on: _root.version
          cases:
            'version_id::apoc': u4
            'version_id::thps': u2
      - id: radius
        type: u4
      - id: bbox
        type: bounding_box
        
      # this value is not present in apocalypse and th1 beta
      - id: unk
        type: u4

      - id: vertices
        type: vec4s2
        repeat: expr
        repeat-expr: num_vertices
      - id: normals
        type: vec4s2
        repeat: expr
        repeat-expr: num_normals
      - id: faces
        type: face
        repeat: expr
        repeat-expr: num_faces
        
  face:
    seq:
      - id: flags
        type: flag
      - id: size
        type: u2
      - id: indices
        type:
          switch-on: _root.version
          cases:
            'version_id::apoc': vec4u2
            'version_id::thps': vec4u1
      - id: color
        type: vec4u1  
      - id: index
        type: u2
      - id: col
        type: u2
      - id: mat_index
        type: u4
        if: flags.is_textured == true
        # UV is using shorts in other versions
      - id: uv
        type: vec2u1
        repeat: expr
        repeat-expr: 4
        if: flags.is_textured == true
      - id: padding1
        type: u4
        if: flags.is_padded == true

  # TODO: outdated, fix
  flag:
    seq:
      - id: is_collision
        type: b1
      - id: is_transparent
        type: b1
      - id: is_padded
        type: b1
      - id: is_quad
        type: b1
      - id: f4
        type: b1
      - id: f3
        type: b1
      - id: f2
        type: b1
      - id: is_textured
        type: b1
      - id: f_rest
        type: b8
      
  vec2u1:
    seq:
      - id: x
        type: u1
      - id: y
        type: u1

  vec4u1:
    seq:
      - id: x
        type: u1
      - id: y
        type: u1
      - id: z
        type: u1
      - id: w
        type: u1

  vec4u2:
    seq:
      - id: x
        type: u2
      - id: y
        type: u2
      - id: z
        type: u2
      - id: w
        type: u2
    
  vec4s2:
    seq:
      - id: x
        type: s2
      - id: y
        type: s2
      - id: z
        type: s2
      - id: w
        type: s2
        
  vec4s4:
    seq:
      - id: x
        type: s4
      - id: y
        type: s4
      - id: z
        type: s4
      - id: w
        type: s4
      
  bounding_box:
    seq:
      - id: xmax
        type: s2
      - id: xmin
        type: s2
      - id: ymax
        type: s2
      - id: ymin
        type: s2
      - id: zmax
        type: s2
      - id: zmin
        type: s2

  texture:
    seq:
    - id: magic
      type: u4
    - id: paltype
      type: u4
    - id: palcrc
      type: u4
    - id: index
      type: u4
    - id: width
      type: u2
    - id: height
      type: u2
    - id: data
      size: stride * height
      
    instances:
      stride:
        # not particularly sure about this one
        value: '(paltype == 16 ? width / 2 + 1 & ~1 : width)'

  texture_set:
    seq:
    - id: num
      type: u4
    - id: ptr
      type: u4
      repeat: expr
      repeat-expr: num
    - id: entries
      type: texture
      repeat: expr
      repeat-expr: num

  palette_set:
    params:
      - id: size
        type: u2
    seq:
    - id: num
      type: u4
    - id: entries
      type: palette(size)
      repeat: expr
      repeat-expr: num

  palette:
    params:
      - id: size
        type: u2
    seq:
    - id: checksum
      type: u4
    - id: entries
      type: color5551
      repeat: expr
      repeat-expr: size

  color5551:
    seq:
    - id: r
      type: b5
    - id: g
      type: b5
    - id: b
      type: b5
    - id: stp
      type: b1

enums:
  ext_type:
    0x06: assumed_tex_coord_anim # wibbly textures
    0x07: assumed_vert_color_anim
    0x0A: block_map # collision table
    0x2A: unknown_2a
    0x2C: anim # animation chunk. sk2anim.psx on th2 mostly
    0x73424752: vertex_clut # a lookup table for gouraud shading
    0x44415551: quad
    0x52454948: hier # models with hierarchy - skaters, cars
    -1: terminator

  version_id:
    3: apoc
    4: thps
    6: xbox