   M S B A M L   `   `   `  ����  xgcalculator!    XGCalculator.BalloonControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      $
��Auto��5      $
��Auto��6     � UseLayoutRounding$
  True��5       � 	Resources 5         BalloonContainerStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Opacity  TitleTextStyle&   System.Windows.Media.TextOptions  EventTextStyle  EventButtonStyle �� 
TargetType	 ��Name
 �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 J    &	     &	 �    �� 5   
   " ��6.   .�5	      ��  � ��
 �����6+   �� 5
       �@��CornerRadius$ 8��6-   �� 5       �@��Padding$ 8q�6(   �� 5       �@J�SnapsToDevicePixels$
 True��64   �� 5       �@J�Effect 5        5      $ 5Q�6'   $ 1Q�66   $
 0.25Q�6F   5      5      �� 5   
   " ��6'   .�5      ��  � ��$ Roboto)�6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 16(�6)   �� 5       � �� �#2226+   �� 5       � ��$ Center=�64   �� 5       �@��TextAlignment$ Center=�6.   �� 5       � ��$ 0,0,0,6q�6'   �� 5       �@ TextFormattingMode$ Display=�6?   �� 5       �@ TextRenderingMode$ 	ClearType=�6>   5!      �� 5$   
   " ��6'   .�5%      ��  � ��$ Inter Light)�6+   �� 5&       � ��$ 14(�6)   �� 5'       � �� �#3336+   �� 5(       � ��$ Center=�64   �� 5)       �@��TextAlignment$ Center=�6.   �� 5*       � ��$ 0,0,0,0q�6'   �� 5+       �@ TextFormattingMode$ Display=�6?   �� 5,       �@ TextRenderingMode$ 	ClearType=�6>   5-      �� 50   
   " ��6)   .�51      ��  � ��
 ���� 6+   �� 52       � ��$ 0q�60   �� 53       � ��$ 4q�6(   �� 54       �@�Cursor$
 Hand��6'   �� 55       � ��$ Center=�64   �� 56       � �� 57      �� 58      " ��6&   .R�59      �� $	 border��6!   #��z��61   $
 4��6[   ._�5:      �� $��Center=�6/   $��Center=�6L   5;       5<      P� 5=       �@J�IsMouseOver$
 True��6=   . �5>   "   �� $ border��6)    � �� �#E0F7FA6S   5?      5A      5C      5D      5E      .��5H      �� #��[  6   ._�5I   
   �� $��Center=�6   .+�5J      �� -   5         ��Name$ RegionTextBlock��5J      #��[ 61   �� 5K      -   5         ��Name$ OptionsPanel��5K       �� Orientation$ Vertical=�6/   $��Center=�6F   5L      5M      5N      