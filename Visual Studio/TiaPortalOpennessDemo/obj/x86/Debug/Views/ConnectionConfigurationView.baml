   M S B A M L   `   `   `  ����  TiaPortalOpennessDemo=    7TiaPortalOpennessDemo.Views.ConnectionConfigurationView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$!  Configure online connection��5   	   $	��179��5   	   $	��493��6    � Icon$D >/TiaPortalOpennessDemo;component/Images/ICO_PE_AppOpenness.ico��5	   	    � Topmost .  � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6t   .��5      � ���#3B4461.+�5   
   �� $��Connection mode:��6   
�������$��24��6>   $	��190��6J   $��	12,12,0,0q�6V   $	��Top=�6i   $
��Left=�6�   �� 5   
    �� DisplayMemberPath$
 Name��60    �� SelectedValuePath$
 Item��6I   $	��Top=�6�   $
��Left=�6�   $��
209,16,0,0q�6�   $	��250��6�   ��6   �� *Mode+ �� SelectedValue 6b   �� *SelectedMode+6�   �� 5   
   $��Connection interface:��6   
�������$��24��6C   $	��190��6O   $��	12,42,0,0q�6[   $	��Top=�6n   $
��Left=�6�   �� 5   
   $
 Name��67   $
 Item��6P   $	��Top=�6�   $
��Left=�6�   $��
209,46,0,0q�6�   $	��250��6�   ��6   �� *PCInterface+ 6i   �� *SelectedInterface+6�   �� 5   
   $"��Connection target interface:��6   
�������$��24��6J   $	��190��6V   $��	12,72,0,0q�6b   $	��Top=�6u   $
��Left=�6�   �� 5   
   $
 Name��62   $
 Item��6K   $	��Top=�6�   $
��Left=�6�   $��
209,76,0,0q�6�   $	��250��6�   ��6   �� *Target+ 6d   �� *SelectedTarget+6�   �� 5   
   $��Cancel��6   $��23��6"   $��Right=�6.   $��0,0,12,9q�6J   $��Bottom=�6\   $��75��6w    �� IsCancel . ��5      �� 5      	 �� EndPoint$	 0.5,1&�6&   
 � MappingMode$
 RelativeToBoundingBox=�67    �� 
StartPoint$ 0.5,0&�6[   .��5      �  � Color$ White��6#   � 5      $ 	#FFDBDCDC��6#    � Offset$ 1Q�65   5      5      �� 5   
   $
��Okay��6   $��Right=�6G   $��0,0,93,9q�6c   $��75��6u   $��23��6�   $��Bottom=�6�   {�6    �� *ConfigureEnabled+��6�   �� *ConfigureConnectionCommand+��5      �� 5      $	 0.5,1&�6&   $
 RelativeToBoundingBox=�67   $ 0.5,0&�6[   .��5      � $ White��6#   � 5      $ 	#FFDBDCDC��6#   $ 1Q�65   5      5!      5"      5#      