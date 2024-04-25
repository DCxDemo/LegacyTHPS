meta:
  id: thps_kat
  application: Tony Hawk's Pro Skater
  title: Tony Hawk's Pro Skater KAT sample container
  file-extension: kat
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyThps/blob/master/formats/thps_kat.ksy

seq:
  - id: num_entries
    type: u4
  - id: entries
    type: entry
    repeat: expr
    repeat-expr: num_entries
  
types:
  entry:
    seq:
      - id: channels
        type: u4
      - id: offset
        type: u4
      - id: size
        type: u4
      - id: freq
        type: u4
      - id: loop
        type: u4
      - id: bits
        type: u4
      - id: unk 
        type: u4
      - id: name # always null, assumed
        size: 16
    instances:
      data:
        size: size
        pos: offset