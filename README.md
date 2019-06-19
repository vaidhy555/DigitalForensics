# Windows Forensic Parser

***Use the link https://github.com/vaidhy555/DigitalForensics/releases/download/v1.2/Windows.Forensic.Parser.Setup.rar to download the required files for the application***. Extract *'Windows.Forensic.Parser.Setup.rar'* and run setup.exe file to install the application.

**To parse MBR**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Master Boot Record - Entire file' or 'Master Boot Record - Partition Entries'* from drop down list;
Click Parse.
The data table displays the MBR vlaues that are parsed from the uploaded file.

![image](https://user-images.githubusercontent.com/51472552/59791901-b64fcc00-92f0-11e9-9824-24d572e68bdc.png)

![image](https://user-images.githubusercontent.com/51472552/59791924-c2d42480-92f0-11e9-9426-b42d3dc7fbaa.png)

**Extract MBR data; do not copy paste hex data in a text file and upload.**

**To parse Directory Entries for FAT file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Directory Table Entries -FAT'* from drop down list; NOTE: Only short file name entries are parsed. Long file name entries aren't parsed. The date time values are in hexadecimal which needs to be converted to MS DOS 32bit timestamp.
Click Parse.

![image](https://user-images.githubusercontent.com/51472552/59791979-da131200-92f0-11e9-8a17-f07b68ad7859.png)

**To parse Volume Boot Record for FAT file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Volume Boot Record - FAT12/FAT16' or 'Volume Boot Record - FAT32'*  from drop down list; 
Click Parse.

![image](https://user-images.githubusercontent.com/51472552/59792045-f747e080-92f0-11e9-8c16-39545546ce1f.png)

The results can be exported to excel sheet.

**To parse Volume Boot Record for NTFS file systems**
Select the binary file to be parsed using 'Browse Button'.
Select the value *'Volume Boot Record - NTFS'*  from drop down list; 
Click Parse.

![image](https://user-images.githubusercontent.com/51472552/59792087-0cbd0a80-92f1-11e9-8e76-74d6ae5242e8.png)

The results can be exported to excel sheet.
