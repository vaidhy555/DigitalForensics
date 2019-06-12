# DigitalForensics
**To parse MBR**
Select the binary file to be parsed using 'Browse Button'.
Select the value from drop down list; NOTE: Only Master Boot Record is supported for now.
Click Parse.
The data table displays the MBR vlaues that are parsed from the uploaded file.

**To parse Directory Entries for FAT file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value from drop down list; NOTE: Only short file name entries are parsed. Long file name entries aren't parsed. The date time values are in hexadecimal which needs to be converted to MS DOS 32bit timestamp.
Click Parse.

The results can be exported to excel sheet.

**Extract MBR data; do not copy paste hex data in a text file and upload.**
