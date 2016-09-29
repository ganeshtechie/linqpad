<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	 List<ItemType> list = new List<ItemType>();
        list.Add(new Clubs() { ClubsArray = new Club[] { new Club() { Num = 0, ClubName = "Some Name", ClubChoice = "Something Else" } } });
        list.Add(new Gift() { Val = "MailGreeting", GiftName = "MailGreeting", GiftDescription = "GiftDescription", GiftQuanity = 1});
        Order order = new Order();
        order.ItemTypes = list.ToArray();

        XmlSerializer serializer = new XmlSerializer(typeof(Order));
        StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stuff.xml");
        serializer.Serialize(sw, order);
        sw.Close();
}


// Define other methods and classes here

[XmlRoot("order")]
public class Order
{
    private ItemType[] itemTypes;
    [XmlElement("ItemType")]
    public ItemType[] ItemTypes
    {
        get { return itemTypes; }
        set { itemTypes = value; }
    }
}

[XmlInclude(typeof(Clubs))]
[XmlInclude(typeof(Gift))]
public abstract class ItemType
{
    private string type = "None";
    [XmlAttribute]
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
}

public class Clubs : ItemType
{
    public Clubs()
    {
        Type = "Clubs";
    }

    private Club[] clubsArray;
    [XmlElement("Club")]
    public Club[] ClubsArray
    {
        get { return clubsArray; }
        set { clubsArray = value; }
    }

}

public class Club
{
    private int num = 0;
    [XmlAttribute("num")]
    public int Num
    {
        get { return num; }
        set { num = value; }
    }

    private string clubName = "";
    public string ClubName
    {
        get { return clubName; }
        set { clubName = value; }
    }

    private string clubChoice = "";
    public string ClubChoice
    {
        get { return clubChoice; }
        set { clubChoice = value; }
    }
}

public class Gift : ItemType
{
    public Gift()
    {
        Type = "Gift";
    }

    private string val = "";
    [XmlAttribute("val")]
    public string Val
    {
        get { return val; }
        set { val = value; }
    }

    private string giftName = "";
    public string GiftName
    {
        get { return giftName; }
        set { giftName = value; }
    }

    private string giftDescription = "";
    public string GiftDescription
    {
        get { return giftDescription; }
        set { giftDescription = value; }
    }

    private int giftQuanity = 0;
    public int GiftQuanity
    {
        get { return giftQuanity; }
        set { giftQuanity = value; }
    }
}