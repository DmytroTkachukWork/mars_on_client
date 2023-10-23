public static class CellDataHelper
{
  private static readonly int[] ARRAY_LONG        = new int[2]{ 1, 3 };
  private static readonly int[] ARRAY_CORNER      = new int[2]{ 0, 1 };
  private static readonly int[] ARRAY_LONG_CORNER = new int[3]{ 0, 1, 3 };
  private static readonly int[] ARRAY_CRIST       = new int[4]{ 0, 1, 2, 3 };
  private static readonly int[] ARRAY_ONE         = new int[1]{ 0 };
  private static readonly int[] ARRAY_NONE        = new int[0]{};

  public static void updateCellData( CellData cell_data )
  {
    int[] dir_int_array = (int[])getDirsArrayByPipeType( cell_data.pipe_type ).Clone();

    if ( dir_int_array.Length == 0 )
      return;

    CellPipeDirection[] dir_type_array = new CellPipeDirection[dir_int_array.Length];

    for ( int i = 0; i < dir_int_array.Length; i++ )
    {
      for( int j = 0; j < (int)cell_data.curent_rotation; j++ )
        dir_int_array[i] = (dir_int_array[i] + 1) % 4;

      dir_type_array[i] = (CellPipeDirection)dir_int_array[i];
    }

    cell_data.all_diractions = dir_type_array;
  }

  private static int[] getDirsArrayByPipeType( CellPipeType pipe_type )
  {
    switch( pipe_type )
    {
    case CellPipeType.LONG        : return ARRAY_LONG;
    case CellPipeType.CORNER      : return ARRAY_CORNER;
    case CellPipeType.LONG_CORNER : return ARRAY_LONG_CORNER;
    case CellPipeType.CRIST       : return ARRAY_CRIST;
    case CellPipeType.ONE         : return ARRAY_ONE;
    default                       : return ARRAY_NONE;
    }
  }
}
