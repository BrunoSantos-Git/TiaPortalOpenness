   M S B A M L   `   `   `  ����P  LTiaPortalOpennessDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TiaPortalOpennessDemo.UtilitiesTiaPortalOpennessDemo.Utilities  5   	   .    (TiaPortalOpennessDemo.Views.SettingsView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   5u-clr-namespace:TiaPortalOpennessDemo.Utilities  5   	     � Title$  Settings��5   	    � SizeToContent$ WidthAndHeight=�6   $	��500��69    � Icon$? 9/TiaPortalOpennessDemo;component/Images/settings_icon.png��5   	    � Topmost .  � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6o    � 	Resources 5      =   7TiaPortalOpennessDemo.Utilities.InverseBooleanConverter   IBConverter��%   &	          5	   
   6$   5
      .��5      �� ���#3B4461.+�5   
   �� $��Main folder path:��6   
�������$��4q�6A   .��5      � ���	#FFF0F0F0.+�5      �� $��5,5,45,5q�6G   $��25��6Y   $	��Top=�6e   ��6   �� *MainFolderPath+ �� Mode$ TwoWay=�6e   �� 5      $��5,5,5,5q�6H   $��25��6Y   $��35��6e   $	��Top=�6p   $��Right=�6�   ��6   �� *StandardMainFolderCommand+��5      �� 5       �� EndPoint$ 0.5,1&�6.   	 � MappingMode$	 RelativeToBoundingBox=�6?   
 �� 
StartPoint$
 0.5,0&�6c   .��5      �  � Color$ White��6+   � 5      $ 	#FFDBDCDC��6+    � Offset$ 1Q�6=   5      .��5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5      5      5      5      �� 5   
   $��Default export path:��6   
�������$��4q�6D   .��5      � ���	#FFF0F0F0.+�5      �� $��5,5,45,5q�6P   $��25��6b   $	��Top=�6n   ��6   �� *DefaultExportFolderPath+$ TwoWay=�6n   �� 5      $��5,5,5,5q�6J   $��25��6[   $��35��6g   $	��Top=�6r   $��Right=�6�   ��6   �� *StandardExportFolderCommand+��5       �� 5!      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��5"      � $ White��6+   � 5#      $ 	#FFDBDCDC��6+   $ 1Q�6=   5$      .��5&      �� $ 
Horizontal=�6!   .+�5'      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5(      5)      5*      5+      �� 5,   
   $��Excel paths:��6   
�������$��4q�6<   .��5-      � ���	#FFF0F0F0.+�5.      �� $
��Left=�6   $��15��67   $��5,10,0,0q�6C   $
��Wrap=�6U   $��Schnittstelle path:��6i   $	��Top=�6�   
���   �$��12(�6�   �� 5/      $��
110,5,45,5q�6Q   $��25��6e   $	��Top=�6q   ��6   �� *DefaultSchnittstellePath+$ TwoWay=�6q   �� 50      $��5,5,5,5q�6U   $��25��6f   $��35��6r   $	��Top=�6}   $��Right=�6�   ��6   �� *(&StandardImportSchnittstelleFileCommand+��51      �� 52      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��53      � $ White��6+   � 54      $ 	#FFDBDCDC��6+   $ 1Q�6=   55      .��57      �� $ 
Horizontal=�6!   .+�58      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   59      5:      �� 5<      $
��Left=�6   $��15��67   $��5,40,0,0q�6C   $
��Wrap=�6U   $��PLC DB path:��6i   $	��Top=�6}   
���   �$��12(�6�   �� 5=      $��110,35,45,5q�6I   $��25��6^   $	��Top=�6j   ��6   �� *DefaultPlcDBPath+$ TwoWay=�6j   �� 5>      $��5,35,5,5q�6G   $��25��6Y   $��35��6e   $	��Top=�6p   $��Right=�6�   ��6   �� *StandardPlcDBFileCommand+��5?      �� 5@      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��5A      � $ White��6+   � 5B      $ 	#FFDBDCDC��6+   $ 1Q�6=   5C      .��5E      �� $ 
Horizontal=�6!   .+�5F      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5G      5H      �� 5J      $
��Left=�6   $��15��67   $��5,70,0,0q�6C   $
��Wrap=�6U   $��Sequence path:��6i   $	��Top=�6   
���   �$��12(�6�   �� 5K      $��110,65,45,5q�6L   $��25��6a   $	��Top=�6m   ��6   �� *DefaultSequencePath+$ TwoWay=�6m   �� 5L      $��5,65,5,5q�6J   $��25��6\   $��35��6h   $	��Top=�6s   $��Right=�6�   ��6   �� *StandardSequenceFileCommand+��5M      �� 5N      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��5O      � $ White��6+   � 5P      $ 	#FFDBDCDC��6+   $ 1Q�6=   5Q      .��5S      �� $ 
Horizontal=�6!   .+�5T      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5U      5V      �� 5X      $
��Left=�6   $��15��67   $��	5,100,0,0q�6C   $
��Wrap=�6V   $��NetworkList path:��6j   $	��Top=�6�   
���   �$��12(�6�   �� 5Y      $��110,95,45,5q�6O   $��25��6d   $	��Top=�6p   ��6   �� *DefaultNetworkListPath+$ TwoWay=�6p   �� 5Z      $��5,95,5,5q�6S   $��25��6e   $��35��6q   $	��Top=�6|   $��Right=�6�   ��6   �� *&$StandartImportNetworkListFileCommand+��5[      �� 5\      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��5]      � $ White��6+   � 5^      $ 	#FFDBDCDC��6+   $ 1Q�6=   5_      .��5a      �� $ 
Horizontal=�6!   .+�5b      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5c      5d      5e      5f      �� 5g   
   $��EPLAN path:��6   
�������$��4q�6;   .��5h      � ���	#FFF0F0F0.+�5i      �� $��5,5,45,5q�6I   $��25��6[   $	��Top=�6g   ��6   �� *DefaultEPlanPath+$ TwoWay=�6g   �� 5j      $��5,5,5,5q�6M   $��25��6^   $��35��6j   $	��Top=�6u   $��Right=�6�   ��6   �� * StandartImportEPlanFileCommand+��5k      �� 5l      $ 0.5,1&�6.   $	 RelativeToBoundingBox=�6?   $
 0.5,0&�6c   .��5m      � $ White��6+   � 5n      $ 	#FFDBDCDC��6+   $ 1Q�6=   5o      .��5q      �� $ 
Horizontal=�6!   .+�5r      �� $;��5/TiaPortalOpennessDemo;component/Images/Scan-icon.png��6    $��20��6_   5s      5t      5u      5v      �� 5w   
   $��TIA Portal user interface��6   
�������$��4q�6I   .��5x      � ���	#FFF0F0F0.+�5y      	� $��On��6   $
��Left=�6c   $	��195��6~   $��20��6�   $	��Top=�6�   $��5,5,0,0q�6�    d� 	IsChecked 6+   �� *UserInterfaceEnabled+$ TwoWay=�6�   	� 5z      $	��Off��6   $
��Left=�6�   $	��195��6�   $��20��6�   $��5,25,0,0q�6�   $	��Top=�6�    6,   �� *UserInterfaceEnabled+ �� 	Converter# [  $ TwoWay=�6�   5{      5|      �� 5}   
   $��Export options��6   
�������$��4q�6>   .��5~      � ���	#FFF0F0F0.+�5      �� $��with defaults��6   $
��Left=�6l   $��20��6�   $	��Top=�6�   $��5,6,0,0q�6�    63   �� *ExportOptionsDefaults+$ TwoWay=�6�   �� 5�      $��with readonly��6   $
��Left=�6l   $��20��6�   $	��Top=�6�   $��5,26,0,0q�6�    63   �� *ExportOptionsReadOnly+$ TwoWay=�6�   5�      5�      � 5�   
   .+�5�      �� $
��Left=�6   $
��Save��60   $��25��6g   $��90��6s   $��	5,5,177,6q�6~   ��6?   �� *SaveSettingsCommand+��5�      �� 5�      $ 0.5,1&�6*   $	 RelativeToBoundingBox=�6;   $
 0.5,0&�6_   .��5�      � $ White��6'   � 5�      $ 	#FFDBDCDC��6'   $ 1Q�69   5�      5�      �� 5�      $��Right=�6   $��Close��61    �� IsCancel . $��25��6Q   $��90��6]   $��	182,4,6,5q�6h   ��5�      �� 5�      $ 0.5,1&�6*   $	 RelativeToBoundingBox=�6;   $
 0.5,0&�6_   .��5�      � $ White��6'   � 5�      $ 	#FFDBDCDC��6'   $ 1Q�69   5�      5�      5�      5�      5�      