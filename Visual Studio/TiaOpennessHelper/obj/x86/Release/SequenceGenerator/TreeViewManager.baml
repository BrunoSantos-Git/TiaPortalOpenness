   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms   TiaOpennessHelper1   +TiaOpennessHelper.ExcelTree.TreeViewManager   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     � Title$  Sequence Generator�� � SizeToContent$ WidthAndHeight=�$	��800�� � 
ResizeMode$ 	CanResize=� � Icon$T Npack://application:,,,/TiaPortalOpennessDemo;component/Images/process_icon.ico��.��� ���#3B4461.+��� -    ��Name$ cbImportToTia��$��Import to TIA��$��	10,82,0,0q�$��20��$	��Top=�$
��Left=�$��14(�$
��Bold"�$��Segoe UI Historic)����	#FFF4F4F4{��� *IsTiaConnected+&   System.Windows.Controls.DataGrid  -     Name$ MainGridView��$��10,103,10,10q�
�������   RowBackground
 �����   AlternatingRowBackground
 �����$��Stretch=�   CanUserReorderColumns .  	 � 	Resources	 
 �� 
TargetType �� Property �� Value��%�   '��       �� "
 ��.���  �@��TextAlignment$ Center=���  �@��TextWrapping$
 Wrap=���  �@��AcceptsReturn . �� -   $��	btn_Clear��$
��Left=�$��
312,10,0,0q�$	��Top=�$��97��$��68������  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&�.���  � Color$ White��� $ 	#FFDBDCDC�� � Offset$ 1Q�.���� .+��� $M��Gpack://application:,,,/TiaPortalOpennessDemo;component/Images/Broom.png��$��35��$��0,5,0,5q��� $��
Clear Grid��$��Center=�$��14(�$��Segoe UI Emoji)��� -   $��btn_GenerateNetNGraf��$
��Left=�$��
211,10,0,0q�$	��Top=�$��97��$��68������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$ 1Q�.���� .+��� $K��Epack://application:,,,/TiaPortalOpennessDemo;component/Images/net.png��$��35��$��0,5,0,5q��� $��NetNGraf��$��Center=�$��14(�$��Segoe UI Emoji)��� -   $��btn_Save��$
��Left=�$��
110,10,0,0q�$	��Top=�$��97��$��68������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$ 1Q�.���� .+��� $L��Fpack://application:,,,/TiaPortalOpennessDemo;component/Images/save.png��$��35��$��0,5,0,5q��� $
��Save��$��Center=�$��14(�$��Segoe UI Emoji)��� -   $��btn_Add��$
��Left=�$��	10,10,0,0q�$	��Top=�$��97��$��68������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�.��� $ White��� $ 	#FFDBDCDC��$ 1Q�.���� .+��� $R��Lpack://application:,,,/TiaPortalOpennessDemo;component/Images/Excel-Icon.png��$��35��$��0,5,0,5q��� $��Create Excel��$��Center=�$��14(�$��Segoe UI Emoji)��� $��415,5,10.4,0q�$��46��$	��Top=�$��Select sheet��
�������.��� .+��� -    ��Name$ 
ComboSheet��$��Stretch=�7  1System.Windows.Forms.Integration.WindowsFormsHost  -	   $��WindowFormSaving��   
FontFamily$ Segoe UI Emoji)�   FontSize$ 14Q�$��415,59,10,0q�$
��Left=�$	��Top=�   
Background �	#FF3B4461   Child.     System.Windows.Forms.Label  -
   
Saving��"   System.Windows.Forms.Control   	ForeColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$ White    	BackColor$ 	#FF3B4461    BorderStyle$
 None=�