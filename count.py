import os
import re
from collections import Counter

chips = {}
for path, subdirs, files in os.walk("."):
    for name in files:
        hdl = re.search(".+\.hdl", name)
        if (hdl):
            f = open(os.path.join(path, name))
            code = f.read()
            chip = re.search("(?<=CHIP\s)\w+(?=\s\{)", code).group()
            parts = re.findall("\w+(?=\s{0,1}\(.*\)\;)", code)
            parts = Counter(parts)
            chips[chip] = parts

values = {"Nand": 1, "DFF": 5, "ARegister": 144, "DRegister": 144}
building = True
while building:
    building = False
    for chip in chips.keys():
        value = 0
        skip = False
        for part in chips[chip].keys():
            if part not in values:
                skip = True
            else:
                value += values[part] * chips[chip][part]
        if not skip:
            values[chip] = value
        else:
            building = True


for value in values.keys():
    print(f"{value}: {values[value]}")
    


            