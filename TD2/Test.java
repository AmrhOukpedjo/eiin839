public class Test{

    public static void main(String[] args) {
        String result = "<html><body> Java test : there are yours args => ";
        for(int i = 0; i < args.length; i++) {
           result += args[i]+"and";
        }
        result += "</body></html>";
    }
}