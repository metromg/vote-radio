import React from 'react';
import { shallow, mount } from 'enzyme';
import { VotingInfo } from './VotingInfo';

describe('VotingInfo', () => {
    it('renders without crashing', () => {
        shallow(<VotingInfo voteCount={0} />);
    });

    describe('with 0 votes', () => {
        it('uses plural', () => {
            // Arrange
            const element = <VotingInfo voteCount={0} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toBe('<div class=\"voting-info\"><span>0&nbsp;Missing translationId: votes for language: ${ languageCode }</span></div>');
        });
    });

    describe('with 1 vote', () => {
        it('uses singular', () => {
            // Arrange
            const element = <VotingInfo voteCount={1} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toBe('<div class=\"voting-info\"><span>1&nbsp;Missing translationId: vote for language: ${ languageCode }</span></div>')
        });
    });

    describe('with more votes', () => {
        it('uses plural', () => {
            // Arrange
            const element = <VotingInfo voteCount={2} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toBe('<div class=\"voting-info\"><span>2&nbsp;Missing translationId: votes for language: ${ languageCode }</span></div>');
        });
    });

    describe('with vote count change of 0', () => {
        it('does not render badge', () => {
            // Arrange
            const element = <VotingInfo voteCount={2} voteCountChange={0} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).not.toContain("badge");
        });
    });

    describe('with vote count change of value', () => {
        it('renders vote count change of positive number', () => {
            // Arrange
            const element = <VotingInfo voteCount={2} voteCountChange={1} />;

            // Act
            const result = mount(element);

            // Assert
            const badge = result.find('.badge');

            expect(badge.text()).toBe('+1');
        });

        it('renders vote count change of negative number', () => {
            // Arrange
            const element = <VotingInfo voteCount={2} voteCountChange={-1} />;

            // Act
            const result = mount(element);

            // Assert
            const badge = result.find('.badge');

            expect(badge.text()).toBe('-1');
        });
    });
});