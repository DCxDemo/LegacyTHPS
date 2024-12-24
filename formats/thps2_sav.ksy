meta:
  id: thps2_sav
  application: Tony Hawk's Pro Skater 2 (PC)
  title: Tony Hawk's Pro Skater 2 (PC) save file
  file-extension: sav
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyTHPS/blob/master/formats/thps_sav.ksy

seq:
  - id: magic # "SC" - magic word
    type: u2
  - id: version # 0x112 - save file version?
    type: u2
  - id: name
    type: strz
    encoding: ascii
    size: 0x5C
  - id: icon # likely memcard icon leftover from psx?
    size: 0x1A4
  - id: char_career
    type: char_career_entry
    repeat: expr
    repeat-expr: 20 # 16 chars + 4 cas ! 24 in korean thps
  - id: score_table
    type: score_table_level
    repeat: expr
    repeat-expr: 14
  - id: gap_mask
    type: gap_mask
    repeat: expr
    repeat-expr: 13 # 8 levels + 2 hidden + 3 legacy?
  - id: skipdata
    size: 0x1a0
  - id: horse_name
    type: strz
    encoding: ascii
    size: 16
  - id: cas
    type: cas_entry
    repeat: expr
    repeat-expr: 4

types:
  char_career_entry:
    seq:
      - id: career_flags
        type: u4
      - id: total_cash # total amount of money
        type: u4
      - id: remaining_cash # cash available to spend
        type: u4  
      - id: level_flags # 2 bytes per level, 10 bits - goals, 3 bits - medals, maybe more?
        type: u2
        repeat: expr
        repeat-expr: 13
      - id: unk
        size: 10
      - id: value2
        type: u4
      - id: decks # bit flag per unlocked deck
        type: u4
      - id: stats # just byte per stat
        size: 10
      - id: rest_of_data # more data
        size: 0xc2

  score_table_level:
    seq: 
      - id: entries
        type: score_table_entry
        repeat: expr
        repeat-expr: 8

  score_table_entry:
    seq:
      - id: label
        type: str
        encoding: ascii
        size: 3
      - id: skater
        type: u1
      - id: score
        type: u4

  gap_mask: # allows for 12*8=96 gaps per level
    seq:
      - id: data
        size: 12

  cas_entry:
    seq:
      - id: name
        type: strz
        encoding: ascii
        size: 16
      - id: hometown
        type: strz
        encoding: ascii
        size: 16
      - id: data
        size: 0x60