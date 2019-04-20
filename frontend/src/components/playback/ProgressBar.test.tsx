import React from 'react';
import { shallow, mount } from 'enzyme';
import { ProgressBar } from './ProgressBar';

describe('ProgressBar', () => {
    it('renders without crashing', () => {
        shallow(<ProgressBar totalDurationInSeconds={0} remainingDurationInSeconds={0} />);
    });

    it('calculates correct percentage', () => {
        // Arrange
        const element = <ProgressBar totalDurationInSeconds={200} remainingDurationInSeconds={100} />;

        // Act
        const result = mount(element);

        // Assert
        const bar = result.find('.bar');

        expect(bar.get(0).props.style).toHaveProperty('width', '50%');
    });

    it('contains correct time string', () => {
        // Arrange
        const element = <ProgressBar totalDurationInSeconds={200} remainingDurationInSeconds={69} />;

        // Act
        const result = mount(element);

        // Assert
        expect(result.text()).toBe('01:09');
    });
});