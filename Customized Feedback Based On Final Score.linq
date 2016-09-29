<Query Kind="Program" />

void Main()
{

	string feedbackSettings = "<Feedback><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed! Try next time.</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Score>10</Score><Message>Congratulations. You've passed.</Message></Grade></Grades></Feedback>";
	
	decimal yourScore = 10.0m;
	
	XElement element = XElement.Parse(feedbackSettings);
	
	string grade = string.Empty;
	
	string message = string.Empty;
	
	var defaultGrade = element.Descendants("Grade")
							.Where(_grade => _grade.Element("IsDefault").Value == "true")
							.Select(_grade => _grade).FirstOrDefault();
	
	// ordering your grades to descending will helpful to find the grade
	// because we are only using >=
	var nonDefaultGrade = element.Descendants("Grade")
			.Where(_grade => _grade.Element("IsDefault").Value == "false")
			.OrderByDescending(_grade => Convert.ToInt32(_grade.Element("Score").Value))
			.Select(_grade => _grade);
	

	int noOfGrades = nonDefaultGrade.Count();
	
	// At this point, we just need to sent the default grade & its message
	// By default, assign the default grade name & message to the source variable.
	// Because, if none of the other grades are eligible for the user, this will be taking precedence.
	grade = defaultGrade.Element("Name").Value;
	message = defaultGrade.Element("Message").Value;


	foreach(var ndg in nonDefaultGrade)
	{
		decimal gradeScore = Convert.ToDecimal(ndg.Element("Score").Value);
		
		// The feedback system ui is designed in such a way that, the only condition to get a grade is
		// the user has to get a score which is greater than or equal to the grade score
		if(yourScore >= gradeScore) { grade = ndg.Element("Name").Value; message = ndg.Element("Message").Value; break; }
	}
	
	
	// This feedbackXml has the information that we need to show to the participant
	XElement feedbackXml = new XElement("Feedback", new XElement("YourScore", yourScore), new XElement("Grade", grade), new XElement("Message", new XCData(message)));
	
	Console.WriteLine(feedbackXml);
		
	
	
}

// Define other methods and classes here