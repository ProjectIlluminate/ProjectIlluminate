   M S B A M L   `   `   `  ����  Project Illuminate UI+    %Project_Illuminate_UI.Pages.StartPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5      :d2http://schemas.microsoft.com/expression/blend/2008  5        N� Title$  	StartPage��5	      .��5      � .+�5   
   � $��30,30,10,10q�6    � RowDefinitions 5      �� 5      $��2* �6    �� 5      $	��11* �6    �� 5      $	��13* �6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      �� 5      �� 5      .+�5      �� $��1q�6    �� CornerRadius$ 3��6)   $��50��6:   $	��Top=�6I   $��Center=�6a   $��3��6~   $��1��6�   ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FFA4B3C5��6'    � Offset$ 0Q�69   � 5      $ 	#FFA4B3C5��6'   $ 1Q�69   5      ._�5!      �� $��1q�6   
���   �$ 0��6A   ��5"      
� 5#       
� GradientOrigin$ 0.225,-0.616&�6.   	 
� RadiusY$	 0.863Q�6L   
 
� RadiusX$
 0.757Q�6\   .��5$      � $ 	#EEFFFFFF��6+   � 5%      $ 	#00FFFFFF��6+   $ 0.696Q�6=   5&      ._�5(      �� -   5        $��	btnGoBack��5(      
������ 
���   �$	��150��6[   $	��Top=�6j   $
��Left=�6�   .��5)      �� .+�5*      �� $��/Images/LogoutXSml.png��6$   $��30��6D   $	��100��6S   $	��Top=�6b   5+      5,      5.      5/      �� 51      -   5        $��btnRoomView��51      $��1��6(   $��1��68   $��20,0.177,20,20q�6E   
����� �.��52      ��  �� Orientation$ Vertical=�6   .+�53      �� $��/Images/LiveFeedSml.png��6   $	��200��6>   �� 54      $��	Room View��6    $
��Bold"�61   $��Bottom=�6C   $��Center=�6^   
�������6{   55      56      �� 58      -   5        $��
btnAddUser��58      $��1��6'   $��2��64   $��20,0.177,20,20q�6D   
���  ��.��59      �� $ Vertical=�6   .+�5:      �� $��/Images/userWhiteSml.png��6   $	��110��6>   $	��Top=�6M   �� 5;      $��Add User��6    $
��Bold"�60   $��Bottom=�6B   $��Center=�6]   
�������6z   5<      5=      �� 5?      -   5        $��btnAddAdmin��5?      $��2��6(   $��1��65   $��20q�6E   
��� ���.��5@      �� $ Vertical=�6   .+�5A      �� $��/Images/AddAdmin.png��6   $	��170��6:   $	��Top=�6I   �� 5B      $��Add Administrator��6    $
��Bold"�69   $��Bottom=�6K   $��Center=�6f   
�������6�   5C      5D      �� 5F      -   5        $��btnListOfUsers��5F      $��2��6+   $��2��68   $��20q�6H   
��� � �.��5G      �� $ Vertical=�6   .+�5H      �� $$��/Images/ListUsers1WhiteSml.png��6   $	��190��6D   �� 5I      $��List Of Users��6    $
��Bold"�65   $��Bottom=�6G   $��Center=�6b   
�������6   5J      5K      5M      5N      5O      