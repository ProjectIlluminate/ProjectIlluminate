   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    Project Illuminate UI&    Project_Illuminate_UI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5   	   JControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	     � Title$  Project Illuminate��5   	   $	��350��6$   $	��525��61    � WindowState$ 	Maximized=�6=    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Resources/Icons.xaml?�6%   %    5      (   "MahApps.Metro.Controls.MetroWindow   RightWindowCommands 5      +   %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� $y�Visible=�6   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��20��6    $��20��6+   ��5      ��  �� RelativeSource ��  �� AncestorType$ ButtonL�	 �� Path$	 
Foreground�
 J� OpacityMask
 5      !� 5       j� Stretch$
 Fill=�6*      
appbar_eye !� Visual# [  69   5      �� 5      $��4 0 0 0q�6    $��Center=�61   $��Watching��6L   5      5      �� 5       $y�Visible=�6   .��5!      �� $ 
Horizontal=�6   .+�5"      �� $��20��6    $��20��6+   ��5#      ��  �� $ ButtonL�$	 
Foreground�
 5$      !� 5%      $
 Fill=�6*     appbar_microphone# [ 69   5'      �� 5(      $��4 0 0 0q�6    $��Center=�61   $��	Listening��6L   5)      5*      5+      .��5.      0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -   5          Name$ kinectRegion��5.      .��5/   
   � ��50      
� 51       
� RadiusY$ 0.573Q�6&    
� RadiusX$ 0.667Q�66    
� GradientOrigin$ 
1.01,1.035&�6F   .��52      �  � Color$ 	#FF06183C��6#    � Offset$
 0.81Q�65   � 53      $ 	#FF0D60A3��6#   54      .+�56       � -   5          �Name$ NavigationFrame��56      57      59      5:      