   M S B A M L   `   `   `  ����E  AWypisWyrys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WypisWyrys
WypisWyrys  5      a ]ArcGIS.Desktop.Extensions, Version=12.2.0.0, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86hJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.ExtensionsArcGIS.Desktop.Extensions 5          WypisWyrys.OwnerInfoView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'uiclr-namespace:WypisWyrys     5      a
extensionsJclr-namespace:ArcGIS.Desktop.Extensions;assembly=ArcGIS.Desktop.Extensions     5        � 	Resources  5       �� MergedDictionaries<  6ArcGIS.Desktop.Extensions.DesignOnlyResourceDictionary   Source�� 5   
    5        5      $S Mpack://application:,,,/ArcGIS.Desktop.Framework;component\Themes\Default.xaml?�6:   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ]� $��0��6   ��. -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Local=�66   $��30��6_   $��0,0,0,0q�6k   .+�5      �� .��6   Imię/Imiona6!   �� 5      -   5        $��	firstName��5   2   $
��30px��6   $��200px��6$   $��Right=�6D   6b   5      ]� 5   
   $��1��6   ��. $ Local=�67   $��30��6`   $��
0,30,0,240q�6l   .+�5      �� .��6   
Nazwisko6   �� 5      -   5        $��lastName��5   2   $
��30px��6   $��200px��6$   $��Right=�6B   6`   5      ]� 5    
   $��2��6   ��. $ Local=�67   $��30��6`   $��
0,60,0,210q�6l   .+�5!      �� .��6   Kod pocztowy6"   �� 5"      -   5        $��zipCode��5"   2   $
��30px��6   $��200px��6$   $��Right=�6B   6`   5#      ]� 5$   
   $��3��6   ��. $ Local=�67   $��30��6`   $��
0,90,0,180q�6l   .+�5%      �� .��6   Miejscowość6!   �� 5&      -   5        $��localization��5&   2   $
��30px��6   $��200px��6$   $��Right=�6G   6e   5'      ]� 5(   
   $��4��6   ��. $ Local=�67   $��30��6`   $��0,120,0,150q�6l   .+�5)      �� .��6   Ulica6   �� 5*      -   5        $��
streetName��5*   2   $
��30px��6   $��200px��6$   $��Right=�6E   6c   5+      �� 5,   
   -   5        $��100px��5,      $
��30px��6   $��200,240,0,0q�6-   $��1��6B   .��6d   Dalej6k   �� 5-   
   -   5        $��100px��5-      $
��30px��6   $��100,240,100,0q�6-   $��1��6D   .��6`   Wstecz6h   5.      5/      