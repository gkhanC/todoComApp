namespace todoCOM.View
{
    public static class ViewerRules
    {
        public static int id_lenght = 3;
        public static int is_completed_lengh = 3;
        public static int tag_lengh = 6;
        public static int title_lengh = 24;
        public static int category_lengh = 8;
        public static int create_date_lengh = 8;
        public static int due_date_lengh = 8;

        public static int totalLineLenght => (id_lenght + is_completed_lengh + tag_lengh + category_lengh +
                                              create_date_lengh + due_date_lengh);
    }
}