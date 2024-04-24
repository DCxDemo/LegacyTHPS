meta:
  id: thps2_bon_dc
  application: Tony Hawk's Pro Skater 2 (Dreamcast)
  title: Treyarch THPS mesh file
  file-extension: bon
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyThps/blob/master/formats/thps2_bon_dc.ksy

doc: |
  Describes skater model in BON format found in THPS2 for the Dreamcast.
  File mapped by DCxDemo*.

  Covers version 1 of the format, for xbox versions 3/4 refer to thps2x_bon.ksy
  It stores textures in Dreamcast PVR format.

seq:

  - id: magic
    contents: [Bon, 0]

  - id: version
    type: u4

  - id: num_mats
    type: u4

  - id: materials
    type: material
    repeat: expr
    repeat-expr: num_mats

  - id: num_hier
    type: u4

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
      
      - id: some_flags
        type: u1
        
      - id: num_matrices # ??
        type: u4

      - id: matrix
        type: matrix_um
        repeat: expr
        repeat-expr: num_matrices

      - id: num_children
        type: u4

      - id: children
        type: mesh
        repeat: expr
        repeat-expr: num_children

      - id: unk
        type: f4
        repeat: expr
        repeat-expr: 6

      - id: num_vertices
        type: u4
      - id: vertices
        type: vertex
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

  matrix:
    seq:
      - id: entries
        type: f4
        repeat: expr
        repeat-expr: 9

  matrix_um:
    seq:
      - id: entries
        type: f4
        repeat: expr
        repeat-expr: 11

  bonstring:
    seq:
      - id: length
        type: u1
      - id: content
        type: str
        encoding: ascii
        size: length

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

  vertex:
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