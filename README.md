# Windows Forensic Parser

***Use the link https://github.com/vaidhy555/DigitalForensics/releases/download/v1.1/Windows.Forensic.Parser.Setup.rar to download the required files for the application***. Extract Windows.Forensic.Parser.Setup.rar and run setup.exe file to install the application.

**To parse MBR**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Master Boot Record - Entire file' or 'Master Boot Record - Partition Entries'* from drop down list;
Click Parse.
The data table displays the MBR vlaues that are parsed from the uploaded file.

![image](https://user-images.githubusercontent.com/51472552/59680379-d008ea80-91ee-11e9-90e0-5a19ec4a7bf0.png)

![image](https://user-images.githubusercontent.com/51472552/59615980-27508180-9141-11e9-9d67-68b7e4f5ff5c.png)

**Extract MBR data; do not copy paste hex data in a text file and upload.**

**To parse Directory Entries for FAT file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Directory Table Entries -FAT'* from drop down list; NOTE: Only short file name entries are parsed. Long file name entries aren't parsed. The date time values are in hexadecimal which needs to be converted to MS DOS 32bit timestamp.
Click Parse.

![image](https://user-images.githubusercontent.com/51472552/59616060-536c0280-9141-11e9-8d89-610393e59205.png)

**To parse Volume Boot Record for FAT file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Volume Boot Record - FAT12/FAT16' or 'Volume Boot Record - FAT32'*  from drop down list; 
Click Parse.

![image](https://user-images.githubusercontent.com/51472552/59680656-810f8500-91ef-11e9-862e-3d5fbf9e74e7.png)

The results can be exported to excel sheet.
