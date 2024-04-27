meta:
  id: thps_fnt1
  application: Tony Hawk's Pro Skater 3
  title: Tony Hawk's Pro Skater 3 bitmap font (FNT1)
  file-extension: fnt
  endian: le

doc-ref: https://github.com/DCxDemo/LegacyTHPS/blob/master/formats/thps3_fnt1.ksy

doc: |
  Bitmap font found in several Tony Hawk's Pro Skater games and derivatives, given a FNT1 label.

seq:
  - id: file_size
    type: u4
  - id: num_glyphs
    type: u4
  - id: height
    type: u4
  - id: baseline
    type: u4

  - id: glyphs
    type: glyph
    repeat: expr
    repeat-expr: num_glyphs

  - id: remain_size
    type: u4
  - id: bmp
    type: bitmap
  - id: num_layouts
    type: u4
  - id: layouts
    type: rectangle
    repeat: expr
    repeat-expr: num_layouts
    
types:
  bitmap:
    seq:
    - id: size
      type: vector2s
    - id: bpp
      type: u2
    - id: reserved # assume it's never used
      size: 6
    - id: data
      size: size.x * size.y
    - id: palette
      size: 256 * 4
      
  glyph:
    seq:
      - id: underline
        type: u2
      - id: symbol_code
        type: str
        encoding: utf-16
        size: 2

  rectangle:
    seq:
      - id: position
        type: vector2s
      - id: size
        type: vector2s

  vector2s:
    seq:
      - id: x
        type: u2
      - id: y
        type: u2