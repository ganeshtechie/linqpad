<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
			//string json = "{\"assessment\":{\"title\":\"Health Assessment\",\"description\":\"A Basic health assessment\",\"questions\":[{\"id\":1,\"required\":true,\"randomizeChoice\":true,\"title\":\"1) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"checkbox\",\"scoringAt\":\"question\",\"score\":1,\"choice\":[{\"correct\":false,\"title\":\"Untitled Choice - 1\",\"id\":1},{\"correct\":false,\"title\":\"Untitled Choice - 2\",\"id\":2}],\"tags\":[\"anxiety\",\"depression\"],\"edit\":false},{\"id\":2,\"required\":true,\"randomizeChoice\":true,\"title\":\"2) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"radiobutton\",\"scoringAt\":\"choice\",\"choice\":[{\"correct\":false,\"title\":\"Untitled Choice - 1\",\"id\":1,\"score\":10},{\"correct\":false,\"title\":\"Untitled Choice - 2\",\"id\":2,\"score\":0},{\"correct\":false,\"title\":\"Untitled Choice - 3\",\"id\":3,\"score\":4}],\"tags\":[\"anxiety\"]},{\"id\":3,\"required\":true,\"randomizeChoice\":true,\"title\":\"2) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"singleline\",\"scoringAt\":\"question\",\"score\":1,\"choice\":{\"id\":1,\"fieldformat\":\"number\",\"maximumlength\":100},\"tags\":[\"anxiety\"]}],\"retake\":0,\"scoringMethod\":\"all\",\"feedbackMessage\":\"Thank you for participating\"}}";
			
			//string json = "{\"assessment\":{\"id\":1,\"title\":\"Sample Assessment\",\"description\":\"Sample Description\",\"feedbackMessage\":\"Thank you for submitting the assessment\",\"retakeLimit\":-1,\"questions\":[{\"id\":1,\"choice\":[{\"id\":1,\"title\":\"Untitled Choice 1\",\"score\":1},{\"id\":2,\"title\":\"Untitled Choice 2\",\"score\":1}],\"choiceType\":\"checkbox\",\"required\":true,\"randomizeChoice\":true,\"title\":\"Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"scoringAt\":\"question\",\"score\":1,\"edit\":false},{\"id\":2,\"required\":true,\"randomizeChoice\":true,\"title\":\"Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"checkbox\",\"scoringAt\":\"question\",\"score\":1,\"rhetorical\":true,\"choice\":[{\"correct\":false,\"title\":\"Untitled Choice 1\",\"id\":1},{\"correct\":false,\"title\":\"Untitled Choice 2\",\"id\":2}],\"tags\":[]},{\"id\":3,\"choice\":[{\"id\":1,\"title\":\"Untitled Choice 1\",\"score\":1},{\"id\":2,\"title\":\"Untitled Choice 2\",\"score\":1}],\"choiceType\":\"checkbox\",\"required\":true,\"randomizeChoice\":true,\"title\":\"Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"scoringAt\":\"question\",\"score\":1,\"edit\":false}],\"scoringMethod\":\"any\",\"stagingMethod\":\"one-by-one\"}}";
			
			string json = "{\"assessment\":{\"id\":1,\"title\":\"History\",\"description\":\"About World War II\",\"feedbackMessage\":\"Thank you for participating.\",\"retakeLimit\":0,\"questions\":[{\"id\":1,\"required\":true,\"randomizeChoice\":true,\"title\":\"Adolf Hitler is from which country ?\",\"choiceType\":\"checkbox\",\"scoringAt\":\"question\",\"score\":20,\"choice\":[{\"correct\":false,\"title\":\"USA\",\"id\":1},{\"correct\":true,\"title\":\"Germany\",\"id\":2},{\"correct\":false,\"title\":\"India\",\"id\":3}],\"tags\":[]}],\"feedback\":{\"grade\":[{\"name\":\"Fail\",\"feedback\":\"Sorry, You've failed! Try next time.\",\"isDefault\":true},{\"name\":\"Pass\",\"score\":10,\"feedback\":\"Congratulations. You've passed.\",\"isDefault\":false}]},\"scoringMethod\":\"any\",\"stagingMethod\":\"one-by-one\"}}";

            XmlDocument xml = JsonConvert.DeserializeXmlNode(json);

            var stringWriter = new StringWriter();

            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xml.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
            }

            XElement element = XElement.Parse(stringWriter.GetStringBuilder().ToString());

            List<XElement> multipleChoices = element.Descendants("questions")
                        .Where(x => x.Element("choiceType").Value == "radiobutton" || x.Element("choiceType").Value == "checkbox").ToList();

            List<XElement> factual = element.Descendants("questions")
                        .Where(x => x.Element("choiceType").Value == "singleline" || x.Element("choiceType").Value == "multiline").ToList();


            XmlSerializer serializer = new XmlSerializer(typeof(MultipleChoiceQuestion));

            List<BaseQuestion> questions = new List<BaseQuestion>();

            foreach (XElement ele in multipleChoices)
            {
                using (TextReader reader = new StringReader(ele.ToString()))
                {
                    MultipleChoiceQuestion question = (MultipleChoiceQuestion)serializer.Deserialize(reader);
                    questions.Add(question);
                }
            }

            serializer = new XmlSerializer(typeof(FactualQuestion));

            foreach (XElement ele in factual)
            {
                using (TextReader reader = new StringReader(ele.ToString()))
                {
                    FactualQuestion question = (FactualQuestion)serializer.Deserialize(reader);
                    questions.Add(question);
                }
            }

            serializer = new XmlSerializer(typeof(Assessment));
			
			Assessment assessment;

            using (TextReader reader = new StringReader(element.ToString()))
            {
                assessment = (Assessment)serializer.Deserialize(reader);
                assessment.questions = questions;
            }
			
			var wizard = new XElement("Wizard");
			wizard.Add(new XElement("Title", assessment.title));
			wizard.Add(new XElement("Description", new XCData(assessment.description)));
			wizard.Add(new XElement("FeedbackMessage", new XCData(assessment.feedbackMessage)));
			wizard.Add(new XElement("MaximumRetakeLimit",assessment.retake));
			
			if(assessment.stagingMethod != null)
				wizard.Add(new XElement("StagingMethod", assessment.stagingMethod));
			
			var scoringElement = new XElement("Scoring");
			scoringElement.Add(new XAttribute("ScoringMethod", assessment.scoringMethod));
			
			wizard.Add(scoringElement);
			
			var section = new XElement("Section");
			
			wizard.Add(new XElement("Page", section));
			
			for(int i=0; i < assessment.questions.Count; i++)
			{
				Type type = assessment.questions[i].GetType();
				
				var questionElement = new XElement("Question");
				
				// Attributes
				questionElement.Add(new XAttribute("ID", assessment.questions[i].id));
				questionElement.Add(new XAttribute("Required", assessment.questions[i].required));
				questionElement.Add(new XAttribute("RandomizeChoices", assessment.questions[i].randomizeChoice));
				questionElement.Add(new XAttribute("ScoringAt", assessment.questions[i].scoringAt));
				questionElement.Add(new XAttribute("Score", assessment.questions[i].score));
				
				if(!string.IsNullOrEmpty(assessment.questions[i].rhetorical))
					questionElement.Add(new XAttribute("Rhetorical", assessment.questions[i].rhetorical));
					
				questionElement.Add(new XAttribute("ChoiceType", assessment.questions[i].choiceType));
				
				// Elements
				questionElement.Add(new XElement("Title", new XCData(assessment.questions[i].title)));
				
				
				
				switch(type.ToString())
				{
					case "UserQuery+MultipleChoiceQuestion":
						MultipleChoiceQuestion multipleChoiceQuestion = assessment.questions[i] as MultipleChoiceQuestion;
						for(int c = 0; c < multipleChoiceQuestion.choice.Length; c ++)
						{
							var choiceObject = multipleChoiceQuestion.choice[c];
							var choiceElement = new XElement("Choice", new XAttribute("ID", choiceObject.id));
							choiceElement.Add(new XAttribute("Correct", choiceObject.correct));
							choiceElement.Add(new XAttribute("Score", choiceObject.score));
							choiceElement.Add(new XElement("Title", new XCData(choiceObject.title)));
							choiceElement.Add(new XElement("Value", new XCData(choiceObject.title)));
							questionElement.Add(choiceElement);
						}
					break;
					case "UserQuery+FactualQuestion":
						FactualQuestion facutalQuestion = assessment.questions[i] as FactualQuestion;
						FactualChoice choice = facutalQuestion.choice;
						var chocieElement = new XElement("Choice", new XAttribute("ID", choice.id));
						chocieElement.Add(new XElement("FieldFormat", choice.fieldformat));
						chocieElement.Add(new XElement("MaximumLength", choice.maximumlength));
						questionElement.Add(chocieElement);
					break;
				}
				
				// Adding Tags
				var tags = new XElement("Tags");
				
				//if(!string.IsNullOrEmpty(assessment.questions[i].tags))
//				{
//				
//					for(var t = 0; t < assessment.questions[i].tags.Length; t ++)
//					{
//						var tag = new XElement("Tag", assessment.questions[i].tags[t]);
//						tags.Add(tag);
//					}
//				
//					questionElement.Add(tags);
//				}
				
				section.Add(questionElement);
			}
			
			var doc = new XDocument(wizard);
			
			Console.WriteLine(doc);
			
}

// Define other methods and classes here

        public abstract class BaseQuestion
        {
            [XmlElement("id")]
            public int id { get; set; }
            [XmlElement("required")]
            public bool required { get; set; }
            [XmlElement("randomizeChoice")]
            public bool randomizeChoice { get; set; }
            [XmlElement("title")]
            public string title { get; set; }
            [XmlElement("choiceType")]
            public string choiceType { get; set; }
            [XmlElement("scoringAt")]
            public string scoringAt { get; set; }
            [XmlElement("score")]
            public int score { get; set; }
            [XmlElement("tags")]
            public string[] tags { get; set; }
            [XmlElement("rhetorical")]
            public string rhetorical { get; set; }
        }

        public abstract class BaseChoice
        {
            public int id { get; set; }
        }

        [XmlRoot("assessment")]
        public class Assessment
        {
            [XmlElement("title")]
            public string title { get; set; }
            [XmlElement("description")]
            public string description { get; set; }
            public List<BaseQuestion> questions { get; set; }
            [XmlElement("retake")]
            public int retake { get; set; }
            [XmlElement("scoringMethod")]
            public string scoringMethod { get; set; }
            [XmlElement("feedbackMessage")]
            public string feedbackMessage { get; set; }
			[XmlElement("stagingMethod")]
			public string stagingMethod { get; set; }
        }

        [XmlRoot("questions")]
        public class MultipleChoiceQuestion : BaseQuestion
        {
            [XmlElement("choice")]
            public MultipleChoice[] choice { get; set; }
        }

        [XmlRoot("questions")]
        public class FactualQuestion : BaseQuestion
        {
            [XmlElement("choice")]
            public FactualChoice choice { get; set; }
        }


        public class MultipleChoice : BaseChoice
        {
            public bool correct { get; set; }
            public string title { get; set; }
            public int score { get; set; }
        }

        public class FactualChoice : BaseChoice
        {
            public string fieldformat { get; set; }
            public string maximumlength { get; set; }
        }