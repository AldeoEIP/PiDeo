using Newtonsoft.Json;

namespace UniDeo {

    public class Rootobject {
        public int id { get; set; }
        public bool isUsed { get; set; }
        public Created created { get; set; }
        public Modified modified { get; set; }
        public Item item { get; set; }
    }

    public class Created {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class Modified {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class Item {
        public int id { get; set; }
        public string name { get; set; }
        public string texture { get; set; }
        public int price { get; set; }
        public Created1 created { get; set; }
        public Modified1 modified { get; set; }
    }

    public class Created1 {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class Modified1 {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }


    public class Answer {
        public string BotSay { get; set; }
    }

    public class Knowledge {
        public string Extract { get; set; }
        public string Title { get; set; }
        public string DetailUrl { get; set; }
    }

    public class Noob : TheoricUser {
        public string CompanionName { get; set; }
    }

    public class ServerDateTime {
        public string Date { get; set; }
        [JsonProperty ("timezone_type")]
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }

    public class TheoricUser {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Translation {
        public string TranslatedText { get; set; }
        public string LangTo { get; set; }
        public string LangFrom { get; set; }
    }

    public class User : TheoricUser {
        public int Id { get; set; }
        public string Salt { get; set; }
        public object GradeOther { get; set; }
        public ServerDateTime Created { get; set; }
        public ServerDateTime Modified { get; set; }
    }
}
