   M S B A M L   `   `   `  ����  xgcalculator*    $XGCalculator.ManualCalibrationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Calibração Manual��5   	   $	��300��6#   $	��400��60   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $)��#Calibração Manual da Grande Área��6   $
��Bold"�5      $��0��6"   $��0��6/   $��2��6?   $��0,0,0,10q�6S   �� 5   
   $��Esquerda (0 a 1):��6   $��1��6-   $��0��6:   $��Center=�6J   $	��0,5q�6e   �� 5   
   -   5         ��Name$ txtGrandeLeft��5      $��1��6)   $��1��66   $��5q�6F   �� 5   
   $��Topo (0 a 1):��6   $��2��6)   $��0��66   $��Center=�6F   $	��0,5q�6a   �� 5   
   -   5        $ txtGrandeTop��5      $��2��6(   $��1��65   $��5q�6E   �� 5   
   $��Direita (0 a 1):��6   $��3��6,   $��0��69   $��Center=�6I   $	��0,5q�6d   �� 5   
   -   5        $ txtGrandeRight��5      $��3��6*   $��1��67   $��5q�6G   �� 5   
   $��Fundo (0 a 1):��6   $��4��6*   $��0��67   $��Center=�6G   $	��0,5q�6b   �� 5    
   -   5        $ txtGrandeBottom��5       $��4��6+   $��1��68   $��5q�6H   �� 5"   
   $��5��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�6F   $��Center=�6_   $��0,10,0,0q�6|   .+�5#      �� -   5        $��Salvar��5#      $��80��6>   $��5q�6I   �� 5$      -   5        $��Cancelar��5$      $��80��6B   $��5q�6M   5%      5&      5'      