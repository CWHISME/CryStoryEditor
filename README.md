### 简介

该项目主要是为了设计一个可视化的节点编辑器，用于方面地编辑剧情逻辑。
最核心类似于：“事件(Event)”、“条件(Condition)”、“行为(Action)”三个作为核心逻辑。
因为依赖这种简单的因果关系，是可以组合成复杂的逻辑的。

在CryStoryEditor 采用了类似于“有向图”的结构(简单画一下吧)：

![图](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_map.jpg)

虽然画着像是棵树，不过实际节点与节点之间，是具有方向的，在同一个容器中，任何一个节点都可以前往指定节点。
当一个节点执行完毕后，将会根据节点的运行结果及运行模式，选择下一个执行步骤。

### 帮助

通过菜单“StoryEditor->Open Story Editor”，可以打开主页面，在主页面中，若当前没有选择任何“Story”，则默认显示如下：

![主页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_1.jpg)

在此，可以选择创建一个新的Story，或者手动选择项目中已存在的Story文件。当当前选中文件属于Story时，编辑器将会进入正式的编辑页面，如图：

![编辑主页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_2.jpg)

在这个正式的编辑页面中，顶部有五个按钮。分别是“Main Page”、“Repair”、“Setting”、“Help”、“About”。
*Main Page：即默认主页，也是Story Editor主要功能所在。
*Repair：该页面主要用于修复或者处理丢失数据情况。
*Setting：这个页面可以设置编辑器Debug模式、Story模板等。
*Help：帮助页面。
*About：关于。

在主页中，右键可以新建Mission，双击新建的Mission节点，可以进入Mission节点页面：

![Mission编辑页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_3.jpg)

在Mission编辑页面的空白处双击，可以回到上一页，即Story页面。

另外，选中相应的Story或Mission文件，然后点击菜单的“StoryEditor->Open Value Manager”可以打开变量管理器，在变量管理器中，可以选择增删变量。

![变量管理器](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_7.jpg)

当变量管理中存在变量时，使用节点“SetStory(Mission)Var”节点即可在Mission中进行使用。
所有存在的变量(取决于变量作用范围)都可以使用下拉列表进行选择。
如图：

![使用变量](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DValueManager_8.png)

其它页面：

![Repair页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_Repair_4.jpg)

![Setting页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_Setting_5.jpg)

![Help页面](http://7xp0w0.com1.z0.glb.clouddn.com/%5BCryStoryEditor%5DCryStoryEditor_Help_6.jpg)


<color=#00FF00>①帮助</color>：

1.在每个节点中，通过单击节点右方接口可拖曳出一条线，连接该线的节点将会作为该节点的子节点，
   当该节点执行结束之后，将会自动运行下一个节点，若下一级节点有多个，则同时运行所有子节点。

2.<color=#00FF00>单击</color>节点左方接口可<color=#FF1493>断开</color>连接至该节点的父节点连线(特殊父节点除外，这时可以通过左边的面板进
   行删除)。

3.通过<color=#00FF00>双击</color>节点，可进入节点的下一级（仅限于“容器”节点）。当进入下一级节点之后——比如从首
   页进入了某一个任务节点，则在标题栏的左方（<color=#7FFF00>窗口左上角</color>）将会自动显示一个按钮，点击此按钮
   可返回上一级。当然，如果你愿意的话，<color=#7FFF00>双击页面空白处</color>，也是可以直接返回上一级的。

4.当打开Story出现节点数据损坏（Mission数据是单独另外保存的），请前往“Repair”页面进行查看
   修复。

5.删除节点可以在”Repair“页面进行。

6.在连接节点时，若同时按下“Ctrl”键，则进入“<color=red>强制链接模式</color>”。在该模式下，不会判断下一节点的合
   法性，并强制与节点链接。该模式很可能造成死循环，使用前请确保退出条件的正确性。

<color=#FFFF00>②关于节点颜色的含义</color>：

   连线：

   白色：代表两个节点之间属于正常的流程，父节点执行完毕，自动运行子节点。
   <color=#00FF00>绿色</color>：当一个节点连接至自身父节点的时候，会显示绿色的连线，代表子节点单方面连接至父节点。
            该节点执行完毕，依然会自动执行下一级节点——即：它们将会构成一个循环，请谨慎使用，
            避免构成死循环。
   <color=#00FFFF>粗的青色</color>：运行时才会出现的连线，代表调试信息。当前运行节点的文字颜色会变成绿色，而已运
                  行过的节点，及下一个将会运行的节点连线颜色则会变成比较粗的青色。

   节点：

   <color=#00FF00>文字绿色</color>：代表该节点属于核心节点，即：当运行时，将会首先从核心节点开始运行。可以允
                  许有多个核心节点，效果就是同时并行执行。
   <color=#017bbc>蓝色</color>：代表事件(Event)节点。
   <color=#918763>淡黄色</color>：代表判断(Condition)节点。
   <color=#6A5ACD>石蓝色</color>：代表行为(Action)节点。

   关于节点顶上的图标：

      绿色的 ✔：代表直到该节点运行成功，才会进入下一个节点。若失败，则重复运行。
      红色的 ✘：代表如果该节点运行失败，则直接结束后面的所有节点。
      黄色的 ←：表示如果该节点运行失败，则返回上一个节点。
      蓝色的 →：表示无论节点运行结果如何，都将继续运行下一个节点。
     （注：通过左边的面板，可以修改该运行模式）

<color=red>③注意</color>：

   编辑器本身是不会自动保存的！这是为了避免因不需要的修改造成的问题，你必须点击右上角的按钮
   进行<color=#00FF00>保存</color>！

   <color=red>当出现Mission错误、丢失情况</color>：

   对于Story与Mission之间的链接，是以Story文件为准，当打开Story时，将会读取Story路径下的  
   <color=#B15BFF>Story文件名+“Missions”</color>命名的文件夹，寻找Mission文件。
   所以，如果需要对Story文件进行重命名的话，请将目录下的Missions文件夹也一并重命名。

   因为当这个文件夹的名字与Story不一致的时候，将会导致读取失败，Mission Data数据丢失。如果
   发生类似的情况，都可以前往“Repair”页面进行修复，一般各种链接关系丢失的情况都可以修正，除
   非是文件丢失。

   当出现<color=red>文件丢失</color>的情况，有两个选择：
     1.删除此文件
     2.找回丢失文件，使用链接修复进行修复