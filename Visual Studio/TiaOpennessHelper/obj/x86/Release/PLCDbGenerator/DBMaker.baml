   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms   TiaOpennessHelper+   %TiaOpennessHelper.SafetyMaker.DBMaker   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     � Title$  PLC DB Generator�� � SizeToContent$ WidthAndHeight=�$	��600��$	��300��$	��700�� � Icon$Q Kpack://application:,,,/TiaPortalOpennessDemo;component/Images/tags-icon.png��.��� ���#3B4461 � ColumnDefinitions �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ cbImportToTia��$��Import to TIA��$��	10,15,0,0q�$��20��$	��Top=�$
��Left=�$��14(�$
��Bold"�$��Segoe UI Historic)����	#FFF4F4F4$��0��$	��106��{��� *IsTiaConnected+�� -   $��0��$
��Left=�$	��Top=�$	��120��$��	10,45,0,0q�$��40������  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&�.���  � Color$ White��� $ 	#FFDBDCDC��	 � Offset$	 1Q�.���� 
 �� Orientation$
 
Horizontal=�.+��� $��Create DB  �� �� TextAlignment$ Center=�$��16(�$��Segoe UI Emoji)��� $S��Mpack://application:,,,/TiaPortalOpennessDemo;component/Images/Create-icon.png��$��20���� -   $��0��$
��Left=�$	��Top=�$	��120��$��	10,95,0,0q�$��40������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$	 1Q�.���� $
 
Horizontal=�.+��� $��Save  ��$ Center=�$��16(�$��Segoe UI Emoji)��� $L��Fpack://application:,,,/TiaPortalOpennessDemo;component/Images/save.png��$��20���� -   $��0��$
��Left=�$	��Top=�$	��120��$��
10,145,0,0q�$��40������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$	 1Q�.���� $
 
Horizontal=�.+��� $��Clear Grid  ��$ Center=�$��16(�$��Segoe UI Emoji)��� $M��Gpack://application:,,,/TiaPortalOpennessDemo;component/Images/Broom.png��$��20��7  1System.Windows.Forms.Integration.WindowsFormsHost  -   $��WindowFormSaving��   FontSize$ 14Q�$��10,195,0,10q�$��25��$
��Left=�$	��120��$	��Top=�   
Background �	#FF3B4461   Child.     System.Windows.Forms.Label  -   
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White    	BackColor$ 	#FF3B4461    BorderStyle$
 None=��� $��0��$
 
Horizontal=�.+��� $��140,10,0,10q�$��Stretch=����	#FFDBDCDC  -   $��WindowsForm_Left��$��155,30,2,10q�$��Stretch=�$��0��. '   !System.Windows.Forms.DataGridView    -	   $��WindowsForm��$��
2,30,10,10q�$��Stretch=�$��1��.   