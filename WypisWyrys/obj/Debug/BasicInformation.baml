   M S B A M L   `   `   `  ����E  AWypisWyrys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WypisWyrys
WypisWyrys  5      a ]ArcGIS.Desktop.Extensions, Version=12.2.0.0, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86hJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.ExtensionsArcGIS.Desktop.Extensions 5      %    WypisWyrys.BasicInformationView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'uiclr-namespace:WypisWyrys     5      a
extensionsJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.Extensions     5        � 	Resources  5       �� MergedDictionaries<  6ArcGIS.Desktop.Extensions.DesignOnlyResourceDictionary   Source�� 5   
    5        5      $S Mpack://application:,,,/ArcGIS.Desktop.Framework;component\Themes\Default.xaml?�6:   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ]� $��0��6   ��. -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Local=�66   $��30��6_   .+�5      �� .��6   Znak sprawy6!   �� 5      -   5        $��caseSign��5   N   $
��30px��6   $��200px��6$   $��Right=�62   6`   5      ]� 5   
   $��1��6   ��. $ Local=�66   $��30��6_   $��	0,0,0,240q�6k   .+�5      �� .��6   Miejscowość6!   �� 5      -   5        $��location��5   N   $
��30px��6   $��200px��6$   $��Right=�62   6`   5      ]� 5    
   $��2��6   ��. $ Local=�66   $��30��6_   $��
0,30,0,210q�6k   .+�5!      �� .��6   Gmina6   �� 5"      -   5        $��	community��5"   N   $
��30px��6   $��200px��6$   $��Right=�62   6a   5#      ]� 5$   
   $��3��6   ��. $ Local=�66   $��30��6_   $��
0,60,0,180q�6k   .+�5%      �� .��6   Data utworzenia6%   (  "System.Windows.Controls.DatePicker  5&      -   5        $
��date��5&   -      Text$ Wybierz datę��6   6-   5'      ]� 5(   
   $��4��6   ��. $ Local=�66   $��30��6_   $��
0,90,0,150q�6k   .+�5)      �� .��6   Data wpłynięcia6%     5*      -   5        $��receiveDate��5*   -   $ Wybierz datę��6   6-   5+      ]� 5,   
   $��5��6   ��. $ Local=�66   $��30��6_   $��0,120q�6k   .+�5-      �� .��6   Kwota6   �� 5.      -   5        $��price��5.   N   $
��30px��6   $��200px��6$   $��Right=�62   6]   5/      ]� 50   
   $��6��6   ��. $ Local=�66   $��30��6_   $��
0,150,0,90q�6k   .+�51      �� .��6   Kwota słownie6#   �� 52      -   5        $��price2��52   N   $
��30px��6   $��200px��6$   $��Right=�62   6^   53      ]� 54   
   $��7��6   $��
0,180,0,60q�6!   $
��30px��65   .+�55      �� -   5        .��55   $   Wstecz6,   �� 56      -	   5        .��56   1   Zaświadczenie6@   �� 57      -
   5        .��57   .   Wypis i wyrys6=   58      59      5:      