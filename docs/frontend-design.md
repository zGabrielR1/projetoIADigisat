# Front end design direction

The preferred color references are `PulseDesign.webp` and `Pulse2.webp` in the project root. The earlier lime Pebble palette was explicitly rejected.

Keep the approved dashboard structure and locally bundled Inter typography. Use ink navy backgrounds, slightly lighter navy panels, fine blue-gray borders, white headings, muted slate secondary text, and blue-to-indigo gradients for primary actions and selected navigation. The feature panel stays dark with a restrained blue glow and decorative wave detail; it should not be a solid bright accent block.

Apply the palette consistently to all five front ends, including charts, focus states, dialogs, the editor, and the light theme. Preserve distinct error, warning, and success colors. Do not introduce mock business figures to match the reference.

The shared styles load in order: `pebble.css`, `pebble-layout.css`, then `pulse.css`. Fonts and the decorative SVG are local so the interface works offline.

The user approved this Pulse palette and the refined layout. Preserve them as the baseline for future work. Use the Taste design skill (`Leonxlnx/taste-skill`) when useful for composition, hierarchy, spacing, and visual quality; project preferences and supplied references take precedence over its defaults.

The BI workspace at `/bi` applies Taste's app-screen guidance: a compact heading, a shared KPI strip, data-led charts, real document drill-down, and a separate data-model view. Keep the shared Pulse shell, while using the quieter BI components defined in `bi.css`. The mobile menu keeps Overview, BI, Reports, and More readable rather than squeezing six destinations into the bottom bar.

Light mode now follows `LightModeReference.webp`: white cards and sidebar, a neutral gray canvas, dark ink text, subtle shadows, and deep teal navigation/actions with muted green data accents. Its assistant panel is a white working surface. This user-supplied reference supersedes the blue Pulse light palette. Dark mode retains Pulse. Load `light.css` last on every screen so light-specific rules cover page controls, tables, charts and dialogs consistently.
