import React from 'react';
import { shallow, mount } from 'enzyme';
import { GlobalError } from './GlobalError';

describe('GlobalError', () => {
    it('renders without crashing', () => {
        shallow(<GlobalError errorMessageKey="test" dismiss={() => null} />);
    });

    describe('with no errorKey', () => {
        it('renders nothing', () => {
            // Arrange
            const element = <GlobalError errorMessageKey={null} dismiss={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toBe(null);
        });
    });

    describe('with errorKey', () => {
        it('renders error message', () => {
            // Arrange
            const element = <GlobalError errorMessageKey="testKey" dismiss={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            const errorMessage = result.find('.error-message > span');
            expect(errorMessage.text()).toBe("Missing translationId: testKey for language: ${ languageCode }");
        });

        it('dismisses error message on dismiss', () => {
            // Arrange
            const dismiss = jest.fn();
            const element = <GlobalError errorMessageKey="testKey" dismiss={dismiss} />

            // Act
            const result = mount(element);

            const button = result.find('button');
            button.simulate('click');

            // Assert
            expect(dismiss).toBeCalledTimes(1);
        });
    });
})