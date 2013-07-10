using System.Collections.Generic;

using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;

using NUnit.Framework;

using Shouldly;

namespace CMZero.Web.UnitTests.Labels
{
    public class BaseLabelsViewModelTests
    {
        public class Given_a_BaseLabelsViewModel
        {
            protected BaseLabelsViewModel BaseLabelsViewModel;

            [SetUp]
            public virtual void SetUp()
            {
                BaseLabelsViewModel = new BaseLabelsViewModel();
            }
        }

        public class When_I_call_GetLabel_with_a_valid_label_name : Given_a_BaseLabelsViewModel
        {
            private string _labelnamethatexists;

            private const string Content = "content";

            private string _result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _labelnamethatexists = "LabelNameThatExists";
                BaseLabelsViewModel.Labels = new LabelCollection { ContentAreas = new List<ContentAreaForDisplay> { new ContentAreaForDisplay { Name = _labelnamethatexists, Content = Content } } };
                _result = BaseLabelsViewModel.GetLabel(_labelnamethatexists);
            }

            [Test]
            public void it_should_return_the_content_from_the_collection()
            {
                _result.ShouldBe(Content);
            }
        }

        public class When_I_call_GetLabel_with_a_labelName_not_in_the_collection : Given_a_BaseLabelsViewModel
        {
            private string _labelNameThatDoesNotExist = "LabelNameThatDoesNotExist";

            private string result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                BaseLabelsViewModel.Labels = new LabelCollection();
                result = BaseLabelsViewModel.GetLabel(_labelNameThatDoesNotExist);
            }

            [Test]
            public void it_should_return_a_blank_string()
            {
                result.ShouldBe(string.Empty);
            }
        }
    }
}