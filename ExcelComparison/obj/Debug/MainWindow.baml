   M S B A M L   `   `   `  ����J  FExcelComparison, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ExcelComparisonExcelComparison  5   	   K*clr-namespace:ExcelComparison.UserControlsExcelComparison.UserControls  5   	        ExcelComparison.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:ExcelComparison     5   	   9uc*clr-namespace:ExcelComparison.UserControls     5   	     � Title$#  SiZone Excel Comparison Tools��5	   	   $	��700��6/   $
��1200��6<    � Icon$ 300logo.ico��5   	    � DataContext 5
   	   ��    Locator �� Source# [   �� Path$ MainWindowViewModel� � 	Resources 5       �� 
TargetType �� Property �� Value��%z   'm�       �� 5   
   " m�6   .�5      ��  � ��$
 auto��6&   �� 5       � ��$
 auto��6'   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��10 �6   �� 5      $��28 �6   �� 5      $��* �6   .+�5   
   o� $��1��6   .@�5      m� -   5        $��导入文件��5      
 m� CommandParameter$
 0��6F    m� Command 6%   �� *ClickCommand+6[   m� 5      -   5         m�Name$ compareButton��5      $��比较��6.   $
 1��6[   {�.   6:   �� *ClickCommand+6�   m� 5      -   5        $ 	preButton��5      $��上一个不同��6*   $
 2��6Z   {�.   69   �� *ClickCommand+6�   m� 5      -   5        $ 
nextButton��5      $��下一个不同��6+   $
 3��6[   {�.   6:   �� *ClickCommand+6�   5      � 5   
   $��2��6    � ColumnDefinitions 5       �� 5!      $	��37* �6#   �� 5"      $��1156* �6#   .+�5$      +   %ExcelComparison.UserControls.OverView  -   5          Name$ overViewPage��5$      $��2��60   6G   5%      5&      5'      