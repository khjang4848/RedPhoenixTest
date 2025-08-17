namespace RedPhoenix.Test.Infrastructure.Messages.DataAnnotations;

using FluentAssertions;
using RedPhoenix.Infrastructure.Messages.DataAnnotations;

[TestClass]
public class EnvelopeTest
{
    //[TestMethod]
    //public void sut_implements_IEnvelope()
    //    => typeof(Envelope).Should().Implement<IEnvelope>();

    //[TestMethod]
    //public void constructor_has_guard_clause_against_empty_messageId()
    //{ 
    //    var action = () => new Envelope(Guid.Empty, new object());
    //    action.Should().Throw<ArgumentException>()
    //        .Where(x => x.ParamName == "messageId");
    //}

    //[TestMethod]
    //public void constructor_allows_null_operationId()
    //{
    //    var action = () =>
    //        new Envelope(Guid.NewGuid(), new object(), operationId: null);
    //    action.Should().NotThrow();
    //}

    //[TestMethod]
    //public void constructor_has_guard_clause_against_empty_correlationId()
    //{
    //    var action = () =>
    //        new Envelope(Guid.NewGuid(), new object(), correlationId: Guid.Empty);
    //    action.Should().Throw<ArgumentException>()
    //        .Where(x => x.ParamName == "correlationId");
    //}

    //[TestMethod]
    //public void constructor_allows_null_correlationId()
    //{
    //    var action = () =>
    //        new Envelope(Guid.NewGuid(), new object(), correlationId: null);
    //    action.Should().NotThrow();
    //}
}
