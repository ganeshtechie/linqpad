<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	//string config = "{\"config\":{\"allowed_choice_types\":[{\"title\":\"Multiple Choice\",\"value\":\"checkbox\"},{\"title\":\"Single Choice\",\"value\":\"radiobutton\"}],\"default_choice_type\":\"checkbox\",\"minimum_no_of_questions_required\":\"1\",\"maximum_no_of_questions_allowed\":\"-1\",\"import_questions\":\"yes\",\"all_mandatory_questions\":\"yes\",\"retake_limit\":\"-1\",\"enable_retake\":\"yes\",\"tagging\":\"no\",\"scoring_configuration\":{\"default_score\":\"1\",\"negative_score\":\"no\",\"scoring_at\":[{\"title\":\"Question\",\"value\":\"question\"},{\"title\":\"Choice\",\"value\":\"choice\"}],\"default_scoring_method\":\"any\",\"allow_to_change_scoring_method\":\"yes\"},\"default_question\":\"{0} Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"default_choice\":\"Untitled Choice {0}\"}}";
	
	string cofigWithoutScoring = "{\"config\":{\"allowed_choice_types\":[{\"title\":\"Multiple Choice\",\"value\":\"checkbox\"},{\"title\":\"Single Choice\",\"value\":\"radiobutton\"}],\"default_choice_type\":\"checkbox\",\"minimum_no_of_questions_required\":\"1\",\"maximum_no_of_questions_allowed\":\"-1\",\"import_questions\":\"yes\",\"all_mandatory_questions\":\"yes\",\"retake_limit\":\"-1\",\"enable_retake\":\"yes\",\"tagging\":\"no\",\"scoring_configuration\":null,\"default_question\":\"{0} Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"default_choice\":\"Untitled Choice {0}\"}}";
	
	XmlDocument xml = JsonConvert.DeserializeXmlNode(cofigWithoutScoring);
	
	//Console.WriteLine(xml);
	
	string json = JsonConvert.SerializeXmlNode(xml);
	
	Console.WriteLine(json);

}

// Define other methods and classes here
