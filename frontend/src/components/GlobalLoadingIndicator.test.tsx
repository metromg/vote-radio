import React from 'react';
import { shallow, mount } from 'enzyme';
import { GlobalLoadingIndicator } from './GlobalLoadingIndicator';

describe('GlobalLoadingIndicator', () => {
    it('renders without crashing', () => {
        shallow(<GlobalLoadingIndicator loading={true}><span>Test</span></GlobalLoadingIndicator>);
    });

    describe('when loading', () => {
        it('displays loading spinner', () => {
            // Arrange
            const element = <GlobalLoadingIndicator loading={true}><span>Test</span></GlobalLoadingIndicator>;

            // Act
            const result = mount(element);

            // Assert
            const loadingIndicator = result.find('.loading-indicator');
            expect(loadingIndicator.html()).toContain('loading-indicator');
        });
    });

    describe('when not loading', () => {
        it('displays children', () => {
            // Arrange
            const element = <GlobalLoadingIndicator loading={false}><span>Test</span></GlobalLoadingIndicator>;

            // Act
            const result = mount(element);

            // Assert
            expect(result.text()).toBe('Test');
        });
    });
});