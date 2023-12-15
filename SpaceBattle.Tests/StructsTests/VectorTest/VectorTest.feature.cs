﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpaceBattle.Tests.StructsTests.VectorTest
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "lol")]
    public partial class ВекторFeature : object, Xunit.IClassFixture<ВекторFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "lol"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "VectorTest.feature"
#line hidden
        
        public ВекторFeature(ВекторFeature.FixtureData fixtureData, SpaceBattle_Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "StructsTests/VectorTest", "вектор", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="векторы свободно складываются")]
        [Xunit.TraitAttribute("FeatureTitle", "вектор")]
        [Xunit.TraitAttribute("Description", "векторы свободно складываются")]
        public void ВекторыСвободноСкладываются()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("векторы свободно складываются", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 5
testRunner.Given("вектор с координатами (1, 1)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 6
testRunner.And("вектор с координатами (1, 2)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 7
testRunner.When("происходит сложение векторов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 8
testRunner.Then("получаем вектор (2, 3)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="векторы имеют разную размерность")]
        [Xunit.TraitAttribute("FeatureTitle", "вектор")]
        [Xunit.TraitAttribute("Description", "векторы имеют разную размерность")]
        public void ВекторыИмеютРазнуюРазмерность()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("векторы имеют разную размерность", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 10
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 11
testRunner.Given("вектор с координатами (1, 2, 3)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 12
testRunner.And("вектор с координатами (1, 4)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 13
testRunner.When("происходит сложение векторов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 14
testRunner.Then("возникает IndexOutOfRangeException", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ВекторFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ВекторFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
