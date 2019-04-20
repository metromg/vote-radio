import React from 'react';
import { shallow, mount } from 'enzyme';
import { CoverImage } from './CoverImage';

describe('CoverImage', () => {
    it('should render without crashing', () => {
        shallow(<CoverImage url={null} />)
    });

    describe('with no url', () => {
        it('has no image', () => {
            // Arrange
            const element = <CoverImage url={null} />;

            // Act
            const result = mount(element);

            // Assert
            const cover = result.find('.cover');

            expect(cover.get(0).props.style).toHaveProperty('backgroundImage', undefined);
            expect(cover.text()).toBe('×');
        });
    });

    describe('with url', () => {
        it('has image', () => {
            // Arrange
            const element = <CoverImage url={"http://test.example.org"} />;

            // Act
            const result = mount(element);

            // Assert
            const cover = result.find('.cover');

            expect(cover.get(0).props.style).toHaveProperty('backgroundImage', 'url(http://test.example.org)');
            expect(cover.text()).toBe('');
        });
    });
});