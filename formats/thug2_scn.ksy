meta:
  id: thug2_scn
  application: Tony Hawk's Underground 2 PC
  title: Tony Hawk's Underground 2 PC Scene file
  file-extension: scn.dat
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyTHPS/blob/master/formats/thug2_scn.ksy

seq:
  - id: unk1 # 1
    type: u4
  - id: unk2 # 1
    type: u4
  - id: unk3 # 1
    type: u4

  - id: num_materials
    type: u4
  - id: materials
    type: material
    repeat: expr
    repeat-expr: num_materials

  - id: num_sectors
    type: u4
  #- id: sectors
  #  type: sector
  #  repeat: expr
  #  repeat-expr: num_sectors

types:
  material:
    seq:
      - id: mat_checksum
        type: u4
      - id: mat_name_checksum
        type: u4
      - id: num_passes
        type: u4
      - id: alpha_cutoff
        type: u4
      - id: sorted
        type: u1
      - id: draw_order
        type: f4
      - id: single_sided
        type: u1
      - id: disable_culling
        type: u1
      - id: z_bias
        type: u4
      - id: has_grass
        type: u1
      - id: grass_height
        type: f4
        if: has_grass == 1
      - id: grass_layers
        type: u4
        if: has_grass == 1
      - id: specular_power
        type: f4

      - id: passes
        type: pass
        repeat: expr
        repeat-expr: num_passes
        
  pass:
    seq:
      - id: tex_checksum
        type: u4
      - id: flags
        type: u4
      
      - id: has_color
        type: u1
        
      - id: color
        type: vector3f

      - id: blend_mode
        type: u4
      - id: blend_fixed_alpha
        type: u4
        
      - id: u_addressing
        type: u4
      - id: v_addressing
        type: u4 
        
      - id: emvmap_u_multiply
        type: u4
      - id: emvmap_v_multiply
        type: u4        
 
      - id: filtering_mode
        type: u4    

      - id: skip_some_data
        size: 4 * 8
        if: flags & 1 >= 1
        
      - id: some_packet_skip
        type: some_packet
        if: flags & 2 >= 1
        
      - id: last_int1
        type: u4
      - id: last_int2
        type: u4 
      - id: last_float1
        type: f4
      - id: last_float2
        type: f4 
        
  some_packet:
    seq:
      - id: num_entries
        type: u4
      - id: entries
        type: packet
        repeat: expr
        repeat-expr: num_entries 
        
  packet:
    seq:
      - id: data
        size: 8 * 4
        
  vector3f:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4