   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms   TiaPortalOpennessDemo*   $TiaPortalOpennessDemo.Views.PLC_Taps   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     � Title$  PLC Tags�� � SizeToContent$ WidthAndHeight=�$	��250��$	��800�� � 
ResizeMode$ 	CanResize=� � Icon$; 5/TiaPortalOpennessDemo;component/Images/tags-icon.png��.��� ���	#FF3B4461.+��� -    ��Name$ btnSave��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��110��$��40������  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&�.���  � Color$ White��� $ 	#FFDBDCDC��	 � Offset$	 1Q�.���� 
 �� Orientation$
 
Horizontal=�.+��� $��Save  �� �� TextAlignment$ Center=�$��16(�$��Segoe UI Emoji)��� $;��5/TiaPortalOpennessDemo;component/Images/Save-icon.png��$��20���� -   $ btnClearGrid��$
��Left=�$��
130,10,0,0q�$	��Top=�$	��110��$��40������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$	 1Q�.���� $
 
Horizontal=�.+��� $��Clear  ��$ Center=�$��16(�$��Segoe UI Emoji)��� $7��1/TiaPortalOpennessDemo;component/Images/Broom.png��$��20���� -   $ btnGenerateXML��$
��Left=�$��
250,10,0,0q�$	��Top=�$	��120��$��40������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$	 1Q�.���� $
 
Horizontal=�.+��� $��
Generate  ��$ Center=�$��16(�$��Segoe UI Emoji)��� $:��4/TiaPortalOpennessDemo;component/Images/xml-icon.png��$��20���� -    ��Name$ cbImportToTia��$��Import to TIA��$��380,10,10,0q�$	��Top=�$
��Left=�$��14(�$
��Bold"�$��Segoe UI Historic)����	#FFF4F4F4{��� *IsTiaConnected+7  1System.Windows.Forms.Integration.WindowsFormsHost  -   $��WindowFormSaving��   FontSize$ 14Q�$��380,30,10,0q�$
��Left=�$
��Auto��   
FontFamily$ Segoe UI Emoji)�$	��Top=�   
Background �	#FF3B4461   Child.     System.Windows.Forms.Label  -   
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White    	BackColor$ 	#FF3B4461    BorderStyle$
 None=��� $��
10,60,10,0q�$	��Top=�$��Stretch=����	#FFDBDCDC  -   $��WindowFormPlcTags��$��Stretch=�$��10,75,10,10q�. '   !System.Windows.Forms.DataGridView     Name$ PlcTags��