﻿# DarkSouls 镜头控制
在魂系列游戏中,镜头控制的策略主要有两种不同情况:一是无锁定,玩家可自由旋转.二是锁定目标,镜头的角度和位置取决于玩家和锁定目标的位置.

## 无锁定镜头
首先看GIF,效果如下.

![](https://raw.githubusercontent.com/knightlyj/demo/master/docs/img/ds-nolock.gif)

可以看出来,有以下几个特点:

	1.角色并没有一直处于屏幕中心.

	2.朝着一个方向跑时,镜头不会偏离太多,且偏离程度稳定.

	3.当角色停下来时,随着镜头接近预期位置,镜头运动速度会逐渐降低.


由此推测,镜头追踪速度取决于镜头与角色位置的差异.当角色偏离镜头中心较远时,镜头运动速度与角色速度相同.当角色偏离中心较近时,镜头运动速度也较慢.
这样一来,只要设置适当的参数,角色会稳定在镜头中心的一定范围内,是典型的负反馈模型.


另外看下面只有横向移动和纵向移动,以及斜方向移动的效果.

![](https://raw.githubusercontent.com/knightlyj/demo/master/docs/img/ds-hormove.gif)

可以看出来,横向移动时,镜头位置几乎没有移动,只有角度改变,可以推测出如下镜头追踪算法.
[//]: <> ( todo  算法图)

根据以上分析,在unity中实现类似了下面的代码
[//]: <> ( todo  伪代码)

赶紧运行看看,效果如下

![](https://raw.githubusercontent.com/knightlyj/demo/master/docs/img/shake.gif)

仔细看一下,会发现移动时角色一直在抖动,经过我一段时间(好几天)的分析,发现是因为每次Update间隔中,物理引擎的step次数不一定相同,而角色运动是基于物理引擎,这样每次造成角色与镜头的偏差一直在小范围内抖动.解决方法就比较简单了,代码如下

[//]: <> (todo  fixedcount代码)

为什么不直接把镜头追踪代码写在FixedUpdate里面,而是计数,再到Update里面处理呢?查阅unity文档,可以知道物理引擎的计算是在FixedUpdate之后,如果在FixedUpdate里面追踪,那么镜头追踪角色的位置并不是实际渲染位置,实际测试也会导致一点点抖动.

## 锁定目标时的镜头
还是先看GIF

![](https://raw.githubusercontent.com/knightlyj/demo/master/docs/img/ds-lock.gif)

可以看出来,就是基于角色与目标的位置,计算出镜头期望位置和角度,再平滑运动即可,有了无锁定算法的基础,实现起来非常容易,算法图示如下:

[//]: <> ( todo 算法图)

代码如下:

[//]: <> ( todo 伪代码)

再看看效果

![](https://raw.githubusercontent.com/knightlyj/demo/master/docs/img/noshake-lock.gif)

## 总结
实现了类似魂系列的镜头控制,遇到的主要问题是镜头抖动,最初并没有想到是物理引擎的原因,在尝试了很多方法,也整理了很多数据和经验之后,才发现是物理引擎造成的.
实际玩游戏时,完全想不到仅仅是镜头控制会这么麻烦,Devils in the details?

PS:最近玩了一会<<塞尔达-荒野之息>>,其中镜头处理就更简单,但玩起游戏来,完全注意不到这些区别(￣ー￣〃)
