meta:
  id: thps2x_ddx
  title: Tony Hawk's Pro Skater 2X texture container
  application: Tony Hawk's Pro Skater 2X
  file-extension: ddx
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyTHPS/blob/master/formats/thps2x_ddx.ksy

doc: |
  Texture container found in Original XBOX game Tony Hawk's Pro Skater 2X. 
  Represents a list of DirectX surfaces (DDS).

seq:
  - id: reserved # always 0
    type: u4
  - id: file_size # can use to validate
    type: u4
  - id: data_offset # texture array starts here
    type: u4
  - id: num_files
    type: u4
  - id: files
    type: file_entry
    repeat: expr
    repeat-expr: num_files

types:
  file_entry:
    seq:
      - id: offset
        type: u4
      - id: data_size
        type: u4
      - id: name # includes file extension (.DDS)
        type: strz
        encoding: ascii
        size: 256
    instances:
      data:
        size: data_size
        pos: _root.data_offset + offset