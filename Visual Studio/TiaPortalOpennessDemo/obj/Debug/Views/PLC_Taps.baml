   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	    TiaPortalOpennessDemo*   $TiaPortalOpennessDemo.Views.PLC_Taps   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5          � Title$  PLC Tags��5   	    � SizeToContent$ WidthAndHeight=�6   $	��250��69   $	��800��6I    � 
ResizeMode$ 	CanResize=�6Y    � Icon$; 5/TiaPortalOpennessDemo;component/Images/tags-icon.png��5	   	   .��5      � ���	#FF3B4461.+�5   
   �� -   5         ��Name$ btnSave��5      $
��Left=�6"   $��	10,10,0,0q�6=   $	��Top=�6P   $	��110��6h   $��40��6t   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67    �� 
StartPoint$ 0.5,0&�6[   .��5      �  � Color$ White��6#   � 5      $ 	#FFDBDCDC��6#   	 � Offset$	 1Q�65   5      .��5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� $��Save  ��6    �� TextAlignment$ Center=�6*   $��16(�6A   $��Segoe UI Emoji)�6O   �� 5      $;��5/TiaPortalOpennessDemo;component/Images/Save-icon.png��6   $��20��6W   5      5      �� 5   
   -   5        $ btnClearGrid��5      $
��Left=�6'   $��
130,10,0,0q�6B   $	��Top=�6V   $	��110��6n   $��40��6z   ��5      �� 5      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5      � $ White��6#   � 5      $ 	#FFDBDCDC��6#   $	 1Q�65   5      .��5      �� $
 
Horizontal=�6   .+�5       �� $��Clear  ��6   $ Center=�6+   $��16(�6B   $��Segoe UI Emoji)�6P   �� 5!      $7��1/TiaPortalOpennessDemo;component/Images/Broom.png��6   $��20��6S   5"      5#      �� 5$   
   -   5        $ btnGenerateXML��5$      $
��Left=�6)   $��
250,10,0,0q�6D   $	��Top=�6X   $	��120��6p   $��40��6|   ��5%      �� 5&      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5'      � $ White��6#   � 5(      $ 	#FFDBDCDC��6#   $	 1Q�65   5)      .��5+      �� $
 
Horizontal=�6   .+�5,      �� $��
Generate  ��6   $ Center=�6.   $��16(�6E   $��Segoe UI Emoji)�6S   �� 5-      $:��4/TiaPortalOpennessDemo;component/Images/xml-icon.png��6   $��20��6V   5.      5/      �� 50   
   -   5         ��Name$ cbImportToTia��50      $��Import to TIA��6*   $��380,10,10,0q�6B   $	��Top=�6W   $
��Left=�6�   $��14(�6�   $
��Bold"�6�   $��Segoe UI Historic)�6�   ���	#FFF4F4F4{�6o   �� *IsTiaConnected+6�   7  1System.Windows.Forms.Integration.WindowsFormsHost  51   
   -   5        $��WindowFormSaving��51   )      FontSize$ 14Q�6   $��380,30,10,0q�6A   $��25��6V   $��Stretch=�6b      
FontFamily$ Segoe UI Emoji)�6�   $	��Top=�6�      
Background �	#FF3B4461   Child. 52          System.Windows.Forms.Label  -   5        
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White 52   '      	BackColor$ 	#FF3B4461 69      BorderStyle$
 None=�6O   53      �� 54   
   $��
10,60,10,0q�6   $	��Top=�6(   $��Stretch=�6@   ���	#FFDBDCDC6^     55   
   -   5        $��WindowFormPlcTags��55   9   $��Stretch=�6   $��10,75,10,10q�6R   . 56      '   !System.Windows.Forms.DataGridView     Name$ PlcTags��6   57      58      59      