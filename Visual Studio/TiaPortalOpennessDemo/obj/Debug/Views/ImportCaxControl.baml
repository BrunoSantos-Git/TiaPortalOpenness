   M S B A M L   `   `   `  ����P  LTiaPortalOpennessDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:TiaPortalOpennessDemo.ViewsTiaPortalOpennessDemo.Views  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      L HTiaOpennessHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:TiaOpennessHelper;assembly=TiaOpennessHelperTiaOpennessHelper 5      2    ,TiaPortalOpennessDemo.Views.ImportCaxControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:TiaPortalOpennessDemo.Views     5      9system&clr-namespace:System;assembly=mscorlib     5      Mhelper:clr-namespace:TiaOpennessHelper;assembly=TiaOpennessHelper     5      -   5          >�Name$  parent��5	       � 	Resources 5         CaxImportOptions S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName��%�   &	        S� 5   
   $ 	GetValues��66   "  5       5      M� 5      $ helper:ImportCaxOptions��6   5      5      .��5      ]� ��.   � DataContext 6   ��  �� ElementName$ parent��.+�5   
   �� $��Import CAx file��6   
�������$	��Top=�6=   $��
10,10,10,0q�6R   �� 5   
    �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $��	10,2,10,2q�6C   .+�5      �� $��
File Path:��6   $��95��6)   
�������64   4   .TiaPortalOpennessDemo.Views.FileBrowserControl  5      -   5        	  Name$	 CaxImportPath��5   S   
   Filter$
 AML File|*.aml��6j   $	��220��6�      Path 6'   ��  �� Path$ FilePath� �� Mode$ TwoWay=�6�   5      �� 5   
   $ 
Horizontal=�6   $	��Top=�6.   $��	10,2,10,2q�6C   .+�5      �� $��Import options:��6   
�������$��95��6B   �� 5      $	��220��6�   ��6   ��  �� Source# [   �� SelectedItem 6X   �� $ SelectedOption�6�   5      �� 5   
   $ 
Horizontal=�6   $��Bottom=�6.   $��Right=�6F   $��
10,2,10,10q�6b   .+�5      �� $��Cancel��67   $��75��6H   $��0,0,2,0q�6S   ��6   �� *CancelCommand+��5      �� 5        �� EndPoint$ 0.5,1&�6*    � MappingMode$ RelativeToBoundingBox=�6;    �� 
StartPoint$ 0.5,0&�6_   .��5!      �  � Color$ White��6'   � 5"      $ 	#FFDBDCDC��6'    � Offset$ 1Q�69   5#      5%      �� 5&      $��Import��67   $��75��6H   $��2,0,0,0q�6S   ��6   �� *ImportCommand+��5'      �� 5(      $ 0.5,1&�6*   $ RelativeToBoundingBox=�6;   $ 0.5,0&�6_   .��5)      � $ White��6'   � 5*      $ 	#FFDBDCDC��6'   $ 1Q�69   5+      5-      5.      5/      50      