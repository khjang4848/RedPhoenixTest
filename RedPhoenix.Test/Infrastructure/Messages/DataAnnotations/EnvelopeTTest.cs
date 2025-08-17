namespace RedPhoenix.Test.Infrastructure.Messages.DataAnnotations;

using AutoFixture.Kernel;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using RedPhoenix.Infrastructure.Messages.DataAnnotations;

[TestClass]
public class EnvelopeTTest
{
    [TestMethod]
    public void sut_implements_IEnvelope()
        => typeof(Envelope<>).Should().Implement<IEnvelope>();

    //[TestMethod]
    //public void constructor_has_guard_clause_against_empty_message_id()
    //{
    //    var action = () =>
    //        new Envelope<Message>(
    //            messageId: Guid.Empty,
    //            message: new Message(),
    //            operationId: default,
    //            correlationId: default,
    //            contributor: default);

    //    action.Should().Throw<ArgumentException>().Where(x => x.ParamName == "messageId");
    //}

    //[TestMethod]
    //public void constructor_has_guard_clause_against_null_message()
    //{
    //    var action = () =>
    //    {
    //        var envelope = new Envelope<Message>(Guid.NewGuid(), 
    //            message: null, 
    //            operationId: default, 
    //            correlationId: default,
    //            contributor: default);
    //    };
    //    action.Should().Throw<ArgumentException>().Where(x => x.ParamName == "message");
    //}

    //[TestMethod]
    //public void constructor_has_guard_clause_against_empty_correlation_id()
    //{
    //    Action action = () =>
    //        new Envelope<Message>(
    //            messageId: Guid.NewGuid(),
    //            message: new Message(),
    //            operationId: default,
    //            correlationId: Guid.Empty,
    //            contributor: default);
    //    action.Should().Throw<ArgumentException>().Where(x => x.ParamName == "correlationId");
    //}

    //[TestMethod]
    //public void sut_is_json_serializable()
    //{
    //    var factory = new MethodInvoker(new GreedyConstructorQuery());
    //    var builder = new Fixture();
    //    builder.Customize<Envelope<Message>>(c => c.FromFactory(factory));

    //    var sut = builder.Create<Envelope<Message>>();
    //    var json = JsonConvert.SerializeObject(sut);

    //    var actual = JsonConvert.DeserializeObject<Envelope<Message>>(json);

    //    actual.Should().BeEquivalentTo(sut);

    //}

    public class Message
    {
    }
}
