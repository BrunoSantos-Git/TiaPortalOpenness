   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	    TiaOpennessHelper1   +TiaOpennessHelper.ExcelTree.TreeViewManager   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5          � Title$  Sequence Generator��5   	    � SizeToContent$ WidthAndHeight=�6$   $	��800��6C    � 
ResizeMode$ 	CanResize=�6S    � Icon$T Npack://application:,,,/TiaPortalOpennessDemo;component/Images/process_icon.ico��5	   	   .��5      � ���#3B4461.+�5   
   �� -   5         ��Name$ cbImportToTia��5      $��Import to TIA��6*   $��	10,82,0,0q�6B   $��20��6U   $	��Top=�6a   $
��Left=�6�   $��14(�6�   $
��Bold"�6�   $��Segoe UI Historic)�6�   ���	#FFF4F4F4{�6y   �� *IsTiaConnected+6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ MainGridView��5      $��10,103,10,10q�6)   
�������   RowBackground
 �����   AlternatingRowBackground
 �����$��Stretch=�6�      CanUserReorderColumns .  	 � 	Resources	 5      
 �� 
TargetType �� Property �� Value��%�   '��       �� 5      "
 ��6   .�5      ��  �@��TextAlignment$ Center=�66   �� 5       �@��TextWrapping$
 Wrap=�65   �� 5       �@��AcceptsReturn . 66   5      5      5      �� 5   
   -   5        $��	btn_Clear��5      $
��Left=�6"   $��
312,10,0,0q�6=   $	��Top=�6Q   $��97��6i   $��68��6t   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67    �� 
StartPoint$ 0.5,0&�6[   .��5      �  � Color$ White��6#   � 5      $ 	#FFDBDCDC��6#    � Offset$ 1Q�65   5      .��5      �� .+�5      �� $M��Gpack://application:,,,/TiaPortalOpennessDemo;component/Images/Broom.png��6   $��35��6i   $��0,5,0,5q�6t   �� 5      $��
Clear Grid��6   $��Center=�6.   $��14(�6L   $��Segoe UI Emoji)�6Z   5       5!      �� 5"   
   -   5        $��btn_GenerateNetNGraf��5"      $
��Left=�6/   $��
211,10,0,0q�6J   $	��Top=�6^   $��97��6v   $��68��6�   ��5#      �� 5$      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5%      � $ White��6#   � 5&      $ 	#FFDBDCDC��6#   $ 1Q�65   5'      .��5)      �� .+�5*      �� $K��Epack://application:,,,/TiaPortalOpennessDemo;component/Images/net.png��6   $��35��6g   $��0,5,0,5q�6r   �� 5+      $��NetNGraf��6   $��Center=�6,   $��14(�6J   $��Segoe UI Emoji)�6X   5,      5-      �� 5.   
   -   5        $��btn_Save��5.      $
��Left=�6!   $��
110,10,0,0q�6<   $	��Top=�6P   $��97��6h   $��68��6s   ��5/      �� 50      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��51      � $ White��6#   � 52      $ 	#FFDBDCDC��6#   $ 1Q�65   53      .��55      �� .+�56      �� $L��Fpack://application:,,,/TiaPortalOpennessDemo;component/Images/save.png��6   $��35��6h   $��0,5,0,5q�6s   �� 57      $
��Save��6   $��Center=�6)   $��14(�6F   $��Segoe UI Emoji)�6T   58      59      �� 5:   
   -   5        $��btn_Add��5:      $
��Left=�6    $��	10,10,0,0q�6;   $	��Top=�6N   $��97��6f   $��68��6q   ��5;      �� 5<      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5=      � $ White��6#   � 5>      $ 	#FFDBDCDC��6#   $ 1Q�65   5?      .��5A      �� .+�5B      �� $R��Lpack://application:,,,/TiaPortalOpennessDemo;component/Images/Excel-Icon.png��6   $��35��6n   $��0,5,0,5q�6y   �� 5C      $��Create Excel��6   $��Center=�60   $��14(�6M   $��Segoe UI Emoji)�6[   5D      5E      �� 5F   
   $��415,5,10.4,0q�6   $��46��6)   $	��Top=�65   $��Select sheet��6M   
�������.��5G      � .+�5H      �� -   5         ��Name$ 
ComboSheet��5H      $��Stretch=�6/   6K   5I      5J      7  1System.Windows.Forms.Integration.WindowsFormsHost  5K   
   -	   5        $��WindowFormSaving��5K         
FontFamily$ Segoe UI Emoji)�63      FontSize$ 14Q�6O   $��415,59,10,0q�6]   $��25��6r   $��Stretch=�6~   $	��Top=�6�      
Background �	#FF3B4461   Child. 5L          System.Windows.Forms.Label  -
   5        
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White 5L   '      	BackColor$ 	#FF3B4461 69      BorderStyle$
 None=�6O   5M      5N      5O      