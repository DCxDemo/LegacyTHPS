meta:
  id: thps2x_bon
  application: Tony Hawk's Pro Skater 2X
  title: Treyarch THPS mesh file
  file-extension: bon
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyThps/blob/master/formats/thps2x_bon.ksy

doc: |
  Describes skater model in BON format found in THPS2x for the Original Xbox.
  File mapped by DCxDemo*.
  
  version 1 found on Dreamcast, uses PVR textures, not supported
  versions 3 and 4 found on Xbox, uses DDS textures, supported
  
  dreamcast bon stores vertices and indices in hierarchy
  instead of global arrays, maybe should create a separate ksy for that one
  also uses floats instead of byte for color and some extra float

seq:

  - id: magic
    contents: [Bon, 0]

  - id: version
    type: u4

  - id: num_mats
    type:
      switch-on: version
      cases:
        3: u2
        4: u4

  - id: materials
    type: material
    repeat: expr
    repeat-expr: num_mats

  - id: num_vertices
    type:
      switch-on: version
      cases:
        3: u2
        4: u4

  - id: num_unk2
    type:
      switch-on: version
      cases:
        3: u2
        4: u4

  - id: vertices # array of vetrices
    type: vertex
    repeat: expr
    repeat-expr: num_vertices

  - id: num_indices
    type:
      switch-on: version
      cases:
        3: u2
        4: u4
  - id: indices # array of tristrip indices
    type: u2
    repeat: expr
    repeat-expr: num_indices

  - id: num_hier
    type:
      switch-on: version
      cases:
        3: u2
        4: u4

  - id: hier
    type: mesh
    repeat: expr
    repeat-expr: num_hier

types:

  material:
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
        type: texture
        if: has_texture == 1

  texture:
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

  mesh:
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
        type: mesh
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

  vertex:
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