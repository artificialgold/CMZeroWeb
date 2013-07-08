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
            protected BaseLabelsViewModel _baseLabelsViewModel;

            [SetUp]
            public virtual void SetUp()
            {
                _baseLabelsViewModel = new BaseLabelsViewModel();
            }
        }

        public class When_I_call_GetLabel_with_a_valid_label_name : Given_a_BaseLabelsViewModel
        {
            private string _labelnamethatexists;

            private string content = "content";

            private string result;

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();
                _labelnamethatexists = "LabelNameThatExists";
                _baseLabelsViewModel.Labels = new LabelCollection { ContentAreas = new List<ContentAreaForDisplay> { new ContentAreaForDisplay { Name = _labelnamethatexists, Content = content } } };
                result = _baseLabelsViewModel.GetLabel(_labelnamethatexists);
            }

            [Test]
            public void it_should_return_the_content_from_the_collection()
            {
                result.ShouldBe(content);
            }
        }

        public class When_I_call_GetLabel_with_a_labelName_not_in_the_collection : Given_a_BaseLabelsViewModel
        {
            private string _labelNameThatDoesNotExist = "LabelNameThatDoesNotExist";

            private string result;

            [SetUp]
            public virtual void SetUp()
            {
                base.SetUp();
                _baseLabelsViewModel.Labels = new LabelCollection();
                result = _baseLabelsViewModel.GetLabel(_labelNameThatDoesNotExist);
            }

            [Test]
            public void it_should_return_a_blank_string()
            {
                result.ShouldBe(string.Empty);
            }
        }
    }
}