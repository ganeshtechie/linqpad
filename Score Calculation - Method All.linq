<Query Kind="Program" />

void Main()
{
	string questionXml = "<Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"20\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	// If the assessment xml has "Scoring" tag in it, then set this variable to true
	bool scoringEnabled = true;
	
	// This value needs to be taken from the assesment xml. The attribute "ScoringMethod" can be found in the element named "Scoring"
	string scoringMethod = "any";
	
	string responseXml = "<Question ID=\"1\"><SelectedChoices><Choice><ID>2</ID></Choice></SelectedChoices></Question>";
	
	XElement question = XElement.Parse(questionXml);
	XElement response = XElement.Parse(responseXml);
	
	string scoringAt = question.Attribute("ScoringAt").Value;
	
	// If the correct choice has been selected by the user, then this score value will be given to the user 
	// for that particular question. This is only one case. In other cases, we need to consider the 
	// scoring method, if the type of the choice is checkbox
	int score = Convert.ToInt32(question.Attribute("Score").Value);
	
	string choiceType = question.Attribute("ChoiceType").Value;
	
	
	// Assuming ScoringAt is "question" & Choice Type is "checkboxes" &  Scoring Method is "any"
	
	// choice Ids the user has selected for this question
	List<int> userSelectedChoiceIds = new List<int>();
	
	userSelectedChoiceIds = response.Element("SelectedChoices").Elements("Choice").Select(item => Convert.ToInt32(item.Element("ID").Value)).ToList();
	
	// Here is the answers for this questions
	List<int> correctChoiceIds = new List<int>();
	
	correctChoiceIds = question.Elements("Choice")
							.Where(item => item.Attribute("Correct").Value == "true")
							.Select(item => Convert.ToInt32(item.Attribute("ID").Value)).ToList();
					
	decimal obtainedScore = 0.0m; bool success = true;
	
	Console.WriteLine(userSelectedChoiceIds); Console.WriteLine(correctChoiceIds);
	
	// I am assuming the scoring method is "all", which means on selecting all the correct choices
	// defined by the author, the participant will get the full score
	
	// #1. the no of correct choices given by the participant and author must be equal
	if(userSelectedChoiceIds.Count == correctChoiceIds.Count){
		//#2. all the choice id's in the correct choice id's collection must be present in the user given collections
		foreach(int ccid in correctChoiceIds)
		{
			bool isCorrect = userSelectedChoiceIds.IndexOf(ccid) != -1;
			if(!isCorrect) { success = false; break; }
		}
	} else {
		success = false;
	}
	
	if(success) obtainedScore = (decimal)score;
	
	Console.WriteLine(obtainedScore);
}

// Define other methods and classes here
