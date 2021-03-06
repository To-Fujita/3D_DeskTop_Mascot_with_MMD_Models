# 3D_DeskTop_Mascot_with_MMD_Models
## 1. Description
This is a program for 3D Desk Top Mascot based on the Visual Studio and DX Library. The 3D animations are used following MMD (MikuMikuDance) models and motions.  
This program is created with Windows Form Apprication (.Net Framworks) and C# on Visual Studio 2019.
- MMD Models  
(1) Ayase Hina: ©NTV http://www.ntv.co.jp/ichara/mmd.html  
(2) Kobayashi Saya: ©NTV http://www.ntv.co.jp/ichara/mmd.html  
(3) YumemiNemu: ©DEARSTAGE inc. https://www.vocaloidnews.net/yumemi-nemu-official-mmd-model-released/  
- MMD Motions  
(1) Radio calisthenics No.1 by ガンプラP  
(2) Radio calisthenics No.2 by にぼさない  
(3) Jojo calisthenics by せっけんP  
(4) Rest(Rain) by ふぃぎゅ＠たっち https://twitter.com/anna_mirrors  

These copyrights for models and motions are owned by each party.  

## 2. Demo
After download and unzip the zip file from the "Code" above, please double click the file of "DeskTop_Mascot.sln". Then, you can create an executable file for the Desk Top Mascot as below. If you execute this program, you can change the model or motion, camera position, etc. by right click the Monkey icon at the right side of "TASKBAR".
![Image_1](https://to-fujita.github.io/Images/DeskTopMascot.gif "Image for DesdkTop Mascot")
   
If you want to create new models and new motions for the Desk Top Mascot, please refer to the next steps.  
### Step-1:
To download (or to creat) new models and new motions from MMD download site. Then, rename the motion files as like "model file name" + 3 digit of numbers + ".vmd". Please save those files in the same directory at "Data" folder of DeskTopMascot.  
  
For Example:  
- Download model file: YumemiNemu_mmd.pmx  
- Download motion file -1: Walk.vmd -> Rename to: YumemiNemu_mmd000.vmd  
- Download motion file -2: Run.vmd -> Rename to: YumemiNemu_mmd001.vmd  
- Download motion file -3: Skip.vmd -> Rename to: YumemiNemu_mmd002.vmd  
### Step-2:
To download the latest version of dxlib file at [DXライブラリ置き場 ダウンロード](https://dxlib.xsrv.jp/dxdload.html).   
Use the viewer of "\Tool\DxLibModelViewer\DxLibModelViewer_64bit.exe" to creat ".mv1" file that is combined model file and motion files, to reduce the file read time. 
### Step-3:
Please change the "\Data\DeskTopMascot_Setting.csv" file to your created files. The csv file format is as follows. 
![Image2](https://to-fujita.github.io/Images/DeskTopMascot_Setting.png "Image for Setting of DeskTop Mascot")

## 3. Reference
### English Site
[Learn MikuMikuDance ](https://learnmmd.com)  
[DX Library](http://nagarei.github.io/DxLibEx/index.html)  
[Yumemi Nemu Official MMD Model Released! - VNN](https://www.vocaloidnews.net/yumemi-nemu-official-mmd-model-released/)  

### Japanese Site
[Visual Studio](https://visualstudio.microsoft.com/ja/)  
[MMDのはじめかた](https://w.atwiki.jp/vpvpwiki/pages/187.html)  
[DXライブラリ置き場](https://dxlib.xsrv.jp/)  
[デスクトップマスコットの作り方](https://qiita.com/massoumen/items/2985a0fb30472b97a590)  
[MMDダウンロード｜アイキャラ4｜日本テレビ](https://www.ntv.co.jp/ichara/mmd.html)  
[夢眠ネムMMDモデル｜VOCALOID(TM)4](https://nemurion.com/download/)  

## 4. License
MIT

## 5. Author
[T. Fujita](https://github.com/To-Fujita)
