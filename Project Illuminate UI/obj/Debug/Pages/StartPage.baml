   M S B A M L   `   `   `  ����  Project Illuminate UI+    %Project_Illuminate_UI.Pages.StartPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5      :d2http://schemas.microsoft.com/expression/blend/2008  5        N� Title$  	StartPage��5	      .��5      � .+�5   
   � $��30,30,10,10q�6    � RowDefinitions 5      �� 5      $��2* �6    �� 5      $	��11* �6    �� 5      $	��13* �6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� $��1q�6    �� CornerRadius$ 3��6)   $��40��6:   $��Center=�6I   $��Center=�6d   $��3��6�   ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FFA4B3C5��6'    � Offset$ 0Q�69   � 5      $ 	#FFA4B3C5��6'   $ 1Q�69   5      ._�5!      �� $��1q�6   
���   �$ 0��6A   ��5"      
� 5#       
� GradientOrigin$ 0.225,-0.616&�6.   	 
� RadiusY$	 0.863Q�6L   
 
� RadiusX$
 0.757Q�6\   .��5$      � $ 	#EEFFFFFF��6+   � 5%      $ 	#00FFFFFF��6+   $ 0.696Q�6=   5&      ._�5(      �� -   5        $��	btnGoBack��5(      $��Log out��6.   
������ 
�������
���   �$	��150��6�   $��80��6�   $	��Top=�6�   $
��Left=�6�   6�   5)      5*      �� 5.      -   5        $��btnRoomView��5.      $��1��6(   $��1��68   $��20,0.177,20,20q�6E   
����� �.��5/      ��  �� Orientation$ Vertical=�6   .+�50      �� $��/Images/LiveFeedSml.png��6   $	��200��6>   �� 51      $��	Room View��6    $
��Bold"�61   $��Bottom=�6C   $��Center=�6^   
�������6{   52      53      �� 55      -   5        $��
btnAddUser��55      $��1��6'   $��2��64   $��20,0.177,20,20q�6D   
���  ��.��56      �� $ Vertical=�6   .+�57      �� $��/Images/userWhiteSml.png��6   $	��110��6>   $	��Top=�6M   �� 58      $��Add User��6    $
��Bold"�60   $��Bottom=�6B   $��Center=�6]   
�������6z   59      5:      �� 5<      -   5        $��btnAddAdmin��5<      $��2��6(   $��1��65   $��20q�6E   
��� ���.��5=      �� $ Vertical=�6   .+�5>      �� $#��/Images/EmailNoteWhiteSml.png��6   $	��170��6C   $	��Top=�6R   �� 5?      $��Add Administrator��6    $
��Bold"�69   $��Bottom=�6K   $��Center=�6f   
�������6�   5@      5A      �� 5C      -   5        $��btnListOfUsers��5C      $��2��6+   $��2��68   $��20q�6H   
��� � �.��5D      �� $ Vertical=�6   .+�5E      �� $$��/Images/ListUsers1WhiteSml.png��6   $	��190��6D   �� 5F      $��List Of Users��6    $
��Bold"�65   $��Bottom=�6G   $��Center=�6b   
�������6   5G      5H      5J      5K      5L      