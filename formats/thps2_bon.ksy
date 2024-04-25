meta:
  id: thps2_bon
  application: Tony Hawk's Pro Skater 2 (Dreamcast, Xbox)
  title: Treyarch THPS mesh file (BON)
  file-extension: bon
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyThps/blob/master/formats/thps2_bon.ksy

doc: |
  Describes skater model in BON format found in THPS2 ports by Treyarch:
    * THPS2 (Dreamcast)
    * THPS2x (Original Xbox)

  Version 1 found on Dreamcast, uses PVR textures, stores separate buffers per mesh
  Versions 3 and 4 found on Xbox, uses DDS textures, stores data in global buffers

  File mapped by DCxDemo*.

seq:
  - id: magic # "Bon\0" string
    contents: [Bon, 0]

  - id: version # 1, 3 or 4
    type: u4

  - id: data
    type:
      switch-on: version
      cases:
        1: bon_dc
        3: bon_xbox
        4: bon_xbox

types:

  bon_xbox:
    seq:
      - id: num_mats
        type:
          switch-on: _root.version
          cases:
            3: u2
            4: u4
    
      - id: materials
        type: material_xbox
        repeat: expr
        repeat-expr: num_mats
    
      - id: num_vertices
        type:
          switch-on: _root.version
          cases:
            3: u2
            4: u4
    
      - id: num_unk2
        type:
          switch-on: _root.version
          cases:
            3: u2
            4: u4
    
      - id: vertices # array of vetrices
        type: vertex_xbox
        repeat: expr
        repeat-expr: num_vertices
    
      - id: num_indices
        type:
          switch-on: _root.version
          cases:
            3: u2
            4: u4
      - id: indices # array of tristrip indices
        type: u2
        repeat: expr
        repeat-expr: num_indices
    
      - id: num_hier
        type:
          switch-on: _root.version
          cases:
            3: u2
            4: u4
    
      - id: hier
        type: mesh_xbox
        repeat: expr
        repeat-expr: num_hier

  material_xbox:
    seq:
      - id: name
        type: bonstring
      - id: color
        type: color
      - id: unk_float1
        type: f4
      - id: unk_float2
        type: f4
      - id: has_texture
        type: u1
      - id: texture
        type: texture_xbox
        if: has_texture == 1

  texture_xbox:
    seq:
      - id: name
        type: bonstring
      - id: flag1 # always 1, does nothing
        type: u1
      - id: address_u # 0 clamp, 1 wrap, 2 mirror
        type: u1
      - id: address_v # 0 clamp, 1 wrap, 2 mirror
        type: u1
      - id: size
        type: u4
      - id: data
        size: size

  bon_dc:
    seq:
      - id: num_mats
        type: u4
    
      - id: materials
        type: material_dc
        repeat: expr
        repeat-expr: num_mats
    
      - id: num_hier
        type: u4
    
      - id: hier
        type: mesh_dc
        repeat: expr
        repeat-expr: num_hier

  material_dc:
    seq:
      - id: name
        type: bonstring
      - id: color
        type: colorf
      - id: unk_float1
        type: f4
      - id: unk_float2
        type: f4
      - id: unk_float3
        type: f4
      - id: unk_flag
        type: u1
      - id: has_texture
        type: u1
      - id: texture
        type: texture_dc
        if: has_texture == 1

  texture_dc:
    seq:
      - id: name
        type: bonstring
      - id: flag1 # always 1, does nothing
        type: u1
      - id: address_u # 0 clamp, 1 wrap, 2 mirror
        type: u1
      - id: address_v # 0 clamp, 1 wrap, 2 mirror
        type: u1
      - id: size
        type: u4
      - id: data
        size: size

  mesh_dc:
    seq:
      - id: entry_type
        type: u1
      - id: name
        type: bonstring
      
      - id: some_flags
        type: u1
        
      - id: num_matrices # ??
        type: u4

      - id: matrix
        type: matrix_dc
        repeat: expr
        repeat-expr: num_matrices

      - id: num_children
        type: u4

      - id: children
        type: mesh_dc
        repeat: expr
        repeat-expr: num_children

      - id: unk
        type: f4
        repeat: expr
        repeat-expr: 6

      - id: num_vertices
        type: u4
      - id: vertices
        type: vertex_dc
        repeat: expr
        repeat-expr: num_vertices
        
      - id: num_splits
        type: u4
      - id: splits
        type: mat_split_dc
        repeat: expr
        repeat-expr: num_splits

  mat_split_dc: 
    seq:
      - id: material_index
        type: u2
      - id: num_indices
        type: u4
      - id: indices
        type: u2
        repeat: expr
        repeat-expr: num_indices

  matrix_dc: # not sure what it stores, xbox only got 1 per node
    seq:
      - id: entries
        type: f4
        repeat: expr
        repeat-expr: 11

  vertex_dc:
    seq:
      - id: position
        type: vector3f
      - id: normal
        type: vector3f
      - id: empty
        type: vector3f
      - id: uv1
        type: vector2f
      - id: uv2
        type: vector2f

  vertex_xbox:
    seq:
      - id: position
        type: vector3f
      - id: unk1 # a lot of 1.0, but also many lower 0.0-1.0 values
        type: f4
      - id: normal # maybe not
        type: vector3f
      - id: wobbliness # apparently vertex wobbliness in the wind
        type: color 
      - id: uv
        type: vector2f

  mesh_xbox:
    seq:
      - id: entry_type
        type: u1
      - id: name
        type: bonstring
      - id: matrix #
        type: matrix
      - id: position # works for most as absolute, except hands, maybe should use matrix to translate
        type: vector3f

      - id: num_children
        type: u2
      - id: children
        type: mesh_xbox
        repeat: expr
        repeat-expr: num_children

      - id: matrix2
        type: matrix
        
      - id: num_base_splits # base mesh parts
        type: u2
      - id: base_splits
        type: mat_split
        repeat: expr
        repeat-expr: num_base_splits
        
      - id: num_joint_splits # stiches in the original engine
        type: u2
      - id: joint_splits
        type: mat_split
        repeat: expr
        repeat-expr: num_joint_splits

  mat_split:
    seq:
      - id: material_index
        type: u2
      - id: offset
        type: u2
      - id: size
        type: u2

  matrix:
    seq:
      - id: entries
        type: f4
        repeat: expr
        repeat-expr: 9

  bonstring:
    seq:
      - id: length
        type:
          switch-on: _root.version
          cases:
            1: u1
            _: u2
      - id: content
        type: str
        encoding: ascii
        size: length

  color:
    seq:
      - id: r
        type: u1
      - id: g
        type: u1
      - id: b
        type: u1
      - id: a
        type: u1

  colorf:
    seq:
      - id: r
        type: f4
      - id: g
        type: f4
      - id: b
        type: f4
      - id: a
        type: f4

  vector4f:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
      - id: w
        type: f4

  vector3f:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4

  vector2f:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4