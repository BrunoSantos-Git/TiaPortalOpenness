   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	    TiaOpennessHelper+   %TiaOpennessHelper.SafetyMaker.DBMaker   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5          � Title$  PLC DB Generator��5	   	    � SizeToContent$ WidthAndHeight=�6"   $	��600��6A   $	��300��6P   $	��700��6`    � Icon$Q Kpack://application:,,,/TiaPortalOpennessDemo;component/Images/tags-icon.png��5
   	   .��5      � ���#3B4461 � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ cbImportToTia��5      $��Import to TIA��6*   $��	10,15,0,0q�6B   $��20��6U   $	��Top=�6a   $
��Left=�6�   $��14(�6�   $
��Bold"�6�   $��Segoe UI Historic)�6�   ���	#FFF4F4F4$��0��6  $	��106��6  {�6y   �� *IsTiaConnected+6  �� 5   
   -   5        $��0��5      $
��Left=�6!   $	��Top=�6<   $	��120��6T   $��	10,45,0,0q�6`   $��40��6�   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67    �� 
StartPoint$ 0.5,0&�6[   .��5      �  � Color$ White��6#   � 5      $ 	#FFDBDCDC��6#   	 � Offset$	 1Q�65   5      .��5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� $��Create DB  ��6    �� TextAlignment$ Center=�6/   $��16(�6F   $��Segoe UI Emoji)�6T   �� 5      $S��Mpack://application:,,,/TiaPortalOpennessDemo;component/Images/Create-icon.png��6   $��20��6o   5      5      �� 5   
   -   5        $��0��5      $
��Left=�6!   $	��Top=�6<   $	��120��6T   $��	10,95,0,0q�6`   $��40��6�   ��5      �� 5       $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5!      � $ White��6#   � 5"      $ 	#FFDBDCDC��6#   $	 1Q�65   5#      .��5%      �� $
 
Horizontal=�6   .+�5&      �� $��Save  ��6   $ Center=�6*   $��16(�6A   $��Segoe UI Emoji)�6O   �� 5'      $L��Fpack://application:,,,/TiaPortalOpennessDemo;component/Images/save.png��6   $��20��6h   5(      5)      �� 5*   
   -   5        $��0��5*      $
��Left=�6!   $	��Top=�6<   $	��120��6T   $��
10,145,0,0q�6`   $��40��6�   ��5+      �� 5,      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5-      � $ White��6#   � 5.      $ 	#FFDBDCDC��6#   $	 1Q�65   5/      .��51      �� $
 
Horizontal=�6   .+�52      �� $��Clear Grid  ��6   $ Center=�60   $��16(�6G   $��Segoe UI Emoji)�6U   �� 53      $M��Gpack://application:,,,/TiaPortalOpennessDemo;component/Images/Broom.png��6   $��20��6i   54      55      7  1System.Windows.Forms.Integration.WindowsFormsHost  56   
   -   5        $��WindowFormSaving��56   )      FontSize$ 14Q�6   $��10,195,0,10q�6A   $��25��6V   $
��Left=�6b   $	��120��6}   $	��Top=�6�      
Background �	#FF3B4461   Child. 57          System.Windows.Forms.Label  -   5        
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White 57   '      	BackColor$ 	#FF3B4461 69      BorderStyle$
 None=�6O   58      �� 59   
   $��0��6   $
 
Horizontal=�6%   .+�5:      �� $��140,10,0,10q�6   $��Stretch=�6-   ���	#FFDBDCDC6I   5;        5<   
   -   5        $��WindowsForm_Left��5<      $��155,30,2,10q�63   $��Stretch=�6H   $��0��6f   . 5=      '   !System.Windows.Forms.DataGridView  5>        5?   
   -	   5        $��WindowsForm��5?      $��
2,30,10,10q�6.   $��Stretch=�6B   $��1��6`   . 5@        5A      5B      5C      