namespace RedPhoenix.Test.Infrastructure.Messages.Abstraction;

using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture;
using FluentAssertions;
using RedPhoenix.Infrastructure.Messages.Abstraction;
using RedPhoenix.Infrastructure.Messages.DataAnnotations;

using Moq;


[TestClass]
public class CompositeMessageHandleTest
{
    [TestMethod]
    public void sut_implements_IMessageHandler()
    {
        var sut = new CompositeMessageHandler();
        sut.Should().BeAssignableTo<IMessageHandler>();
    }

    [TestMethod]
    public void class_has_guard_clauses()
    {
        var builder = new Fixture().Customize(new AutoMoqCustomization());
        new GuardClauseAssertion(builder).Verify(typeof(CompositeMessageHandler));
    }

    [TestMethod]
    public void constructor_sets_Handles_correctly()
    {
        var handler1 = Mock.Of<IMessageHandler>();
        var handler2 = Mock.Of<IMessageHandler>();
        var handler3 = Mock.Of<IMessageHandler>();

        var sut = new CompositeMessageHandler(handler1, handler2, handler3);

        sut.Handlers.Should().Equal(handler1, handler2, handler3);
    }

    [TestMethod]
    public void Accepts_returns_true_if_all_handlers_accept_message()
    {
        var envelope = new Fixture().Create<Envelope>();

        var handler1 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope));
        var handler2 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope));
        var handler3 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope));

        var sut = new CompositeMessageHandler(handler1, handler2, handler3);

        sut.Handlers.Should().Equal(handler1, handler2, handler3);
    }

    [TestMethod]
    [DataRow(true, false, false)]
    [DataRow(false, true, false)]
    [DataRow(false, false, true)]
    [DataRow(true, true, false)]
    [DataRow(true, false, true)]
    [DataRow(false, true, true)]
    public void Accepts_returns_true_if_some_handlers_accept_message(bool accepts1, bool accepts2, bool accepts3)
    {
        var envelope = new Fixture().Create<Envelope>();

        var handler1 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == accepts1);
        var handler2 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == accepts2);
        var handler3 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == accepts3);

        var sut = new CompositeMessageHandler(handler1, handler2, handler3);

        var actual = sut.Accepts(envelope);

        actual.Should().BeTrue();

    }

    [TestMethod]
    public void Accepts_returns_false_if_no_handler_accepts_message()
    {
        var envelope = new Fixture().Create<Envelope>();
        var handler1 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == false);
        var handler2 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == false);
        var handler3 = Mock.Of<IMessageHandler>(x => x.Accepts(envelope) == false);

        var sut = new CompositeMessageHandler(handler1, handler2, handler3);

        var actual = sut.Accepts(envelope);

        actual.Should().BeFalse();
    }

    [TestMethod]
    public async Task Handle_sends_message_to_all_handlers()
    {
        var handler1 = Mock.Of<IMessageHandler>();
        var handler2 = Mock.Of<IMessageHandler>();

        var sut = new CompositeMessageHandler(handler1, handler2);
        var message = new object();
        var envelope = new Envelope(Guid.NewGuid(), message);

        await sut.Handle(envelope, CancellationToken.None);

        Mock.Get(handler1).Verify(x => x.Handle(envelope, CancellationToken.None),
            Times.Once);
        Mock.Get(handler2).Verify(x => x.Handle(envelope, CancellationToken.None),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_sends_message_to_all_handlers_even_if_some_handler_fails()
    {
        var handler1 = Mock.Of<IMessageHandler>();
        var handler2 = Mock.Of<IMessageHandler>();

        var sut = new CompositeMessageHandler(handler1, handler2);
        var message = new object();
        var envelope = new Envelope(Guid.NewGuid(), message);

        Mock.Get(handler1)
            .Setup(x => x.Handle(envelope, CancellationToken.None))
            .Throws<InvalidOperationException>();

        try
        {
            await sut.Handle(envelope, CancellationToken.None);
        }
        catch
        {
        }

        Mock.Get(handler1).Verify(x => x.Handle(envelope, CancellationToken.None),
            Times.Once());
        Mock.Get(handler2).Verify(x => x.Handle(envelope, CancellationToken.None),
            Times.Once());
    }

    [TestMethod]
    public async Task Handle_throws_aggregate_exception_if_handler_fails()
    {
        var handler1 = Mock.Of<IMessageHandler>();
        var handler2 = Mock.Of<IMessageHandler>();

        var sut = new CompositeMessageHandler(handler1, handler2);
        var message = new object();
        var envelope = new Envelope(Guid.NewGuid(), message);

        var exception1 = new InvalidOperationException();
        Mock.Get(handler1)
            .Setup(x => x.Handle(envelope, CancellationToken.None))
            .Throws(exception1);

        var exception2 = new InvalidOperationException();
        Mock.Get(handler2)
            .Setup(x => x.Handle(envelope, CancellationToken.None))
            .Throws(exception2);

        var action = () => sut.Handle(envelope, CancellationToken.None);

        var actionResult = await action.Should().ThrowAsync<AggregateException>();

        var exceptionList = new List<InvalidOperationException>()
            {
                exception1, exception2
            };

        actionResult.Which.InnerExceptions.Should().BeEquivalentTo(exceptionList);
    }

    [TestMethod]
    public void sut_is_not_sealed_class()
        => typeof(CompositeMessageHandler).Should().NotBeSealed();

}
