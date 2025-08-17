namespace RedPhoenix.Test.Infrastructure.Messages.Abstraction;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using RedPhoenix.Infrastructure.Messages.Abstraction;
using RedPhoenix.Infrastructure.Messages.DataAnnotations;

[TestClass]
public class InterfaceAwareHandlerTest
{
    [TestMethod]
    public void class_is_abstract()
        => typeof(InterfaceAwareHandler).IsAbstract.Should().BeTrue();

    [TestMethod]
    public void sut_implements_IMessageHandler()
    {
        var sut = Mock.Of<InterfaceAwareHandler>();
        sut.Should().BeAssignableTo<IMessageHandler>();
    }

    [TestMethod]
    public void sut_has_guard_clauses()
    {
        var builder = new Fixture().Customize(new AutoMoqCustomization());
        var assertion = new GuardClauseAssertion(builder);
        assertion.Verify(typeof(InterfaceAwareHandler));
    }

    public class FooMessage;

    public class BarMessage;

    public class MessageHandler : InterfaceAwareHandler, IHandles<FooMessage>,
        IHandles<BarMessage>
    {
        public Task Handle(Envelope<FooMessage> envelope, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task Handle(Envelope<BarMessage> envelope, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }

    public class UnknownMessage;

    [TestMethod]
    public void sut_implements_IHandles_for_all_interfaces()
    {
        var sut = Mock.Of<MessageHandler>();
        sut.Should().BeAssignableTo<IHandles<FooMessage>>();
        sut.Should().BeAssignableTo<IHandles<BarMessage>>();
    }

    [TestMethod]
    public void Accepts_returns_true_if_sut_handles_message()
    {
        var message = new FooMessage();
        var envelope = new Envelope(Guid.NewGuid(), message);
        var sut = new MessageHandler();

        var actual = sut.Accepts(envelope);

        actual.Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_returns_false_if_sut_does_not_handle_message()
    {
        var message = new UnknownMessage();
        var envelope = new Envelope(Guid.NewGuid(), message);
        var sut = Mock.Of<MessageHandler>();

        var actual = sut.Accepts(envelope);

        actual.Should().BeFalse();
    }

    [TestMethod]
    public void Accepts_is_virtual()
    {
        typeof(InterfaceAwareHandler).GetMethod("Accepts").Should().BeVirtual();
    }


}
