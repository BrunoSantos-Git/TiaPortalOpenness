   M S B A M L   `   `   `  ����P  LTiaPortalOpennessDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TiaPortalOpennessDemo.UtilitiesTiaPortalOpennessDemo.Utilities  5   	   2    ,TiaPortalOpennessDemo.Views.TargetSelectView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C	utilities-clr-namespace:TiaPortalOpennessDemo.Utilities     5   	     � Title$  Select target��5   	   $	��300��5   	   $	��425��6    � Icon$D >/TiaPortalOpennessDemo;component/Images/ICO_PE_AppOpenness.ico��5   	    � Topmost .  � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6t   .��5	      � ���#3B4461.+�5
   
   �� $��Select target: ��6   $	��Top=�6+   $
��Left=�6C   
�������$��	12,10,0,0q�6q   $��Segoe UI Emoji)�6�   $��16(�6�   $
��Bold"�6�   R� 5   
   $��12,37,12,38q�6�   ��6   �� *TargetTree.View+4   .TiaPortalOpennessDemo.Utilities.TreeViewHelper   SelectedItem 6;   �� *TargetTree.SelectedItem+ �� Mode$ TwoWay=�   SelectedItemChanged 6�   �� *TargetTree.SelectedItemChgCmd+ � 	Resources 5      	 �� 
TargetType
 �� Property �� Value �� Color��%�   'R�       �� 5      "	 R�6   .�5      �� 
 � ��$	 0.0q�5   9   5      �� 5      
 � �� 5      �� 5      $ 	#FFF0F0F0��6.   5      5      5      5      �� 5   
   $��Select��69   $��75��6J   $��23��6U   $��Bottom=�6a   $��Right=�6|   $��0,0,93,9q�6�   ��6   �� *SelectTargetCommand+��5      �� 5       �� EndPoint$ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67    �� 
StartPoint$ 0.5,0&�6[   .��5      �  � Color$ White��6#   � 5      $ 	#FFDBDCDC��6#    � Offset$ 1Q�65   5      5       �� 5!   
    �� IsCancel . $��Cancel��6!   $��75��62   $��23��6=   $��Bottom=�6I   $��Right=�6d   $��0,0,12,9q�6�   ��5"      �� 5#      $ 0.5,1&�6&   $ RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5$      � $ White��6#   � 5%      $ 	#FFDBDCDC��6#   $ 1Q�65   5&      5(      5)      5*      